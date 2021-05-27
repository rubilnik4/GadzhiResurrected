using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Extensions.Functional;
using GadzhiCommon.Extensions.Functional.Result;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiCommon.Models.Interfaces.Errors;
using GadzhiMicrostation.Microstation.Interfaces.ApplicationMicrostationPartial;
using GadzhiMicrostationSignatures.Infrastructure.Interfaces;
using GadzhiMicrostationSignatures.Models.Implementations;
using GadzhiApplicationCommon.Extensions.Functional.Result;
using GadzhiMicrostation.Microstation.Interfaces.DocumentMicrostationPartial;
using GadzhiMicrostation.Models.Implementations.Coordinates;
using GadzhiMicrostation.Microstation.Interfaces;
using System.IO;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiApplicationCommon.Models.Implementation.LibraryData;
using GadzhiCommon.Models.Implementations.Functional;
using GadzhiCommon.Models.Implementations.LibraryData;
using GadzhiCommon.Models.Interfaces.LibraryData;
using GadzhiDTOBase.Infrastructure.Implementations.Converters;
using GadzhiMicrostationSignatures.Models.Enums;
using GadzhiConvertingLibrary.Extensions;
using GadzhiConvertingLibrary.Infrastructure.Implementations.Converters;
using GadzhiConvertingLibrary.Infrastructure.Implementations.Services;
using GadzhiConvertingLibrary.Infrastructure.Interfaces.Services;
using GadzhiMicrostationSignatures.Models.Interfaces;

namespace GadzhiMicrostationSignatures.Infrastructure.Implementations
{
    /// <summary>
    /// Преобразование подписей Microstation в Jpeg
    /// </summary>
    public class SignaturesUpload : ISignaturesToJpeg
    {
        public SignaturesUpload(IApplicationMicrostation applicationMicrostation, IProjectSignatureSettings projectSignatureSettings,
                                IMessagingService messagingService, IFileSystemOperations fileSystemOperations,
                                IFilePathOperations filePathOperations, IWcfServerServicesFactory wcfServerServicesFactory)
        {
            _applicationMicrostation = applicationMicrostation ;
            _projectSignatureSettings = projectSignatureSettings;
            _messagingService = messagingService ;
            _fileSystemOperations = fileSystemOperations ;
            _filePathOperations = filePathOperations;
            _signatureServerServiceFactory = wcfServerServicesFactory?.SignatureServerServiceFactory ;
        }

        /// <summary>
        /// Модуль конвертации Microstation
        /// </summary>   
        private readonly IApplicationMicrostation _applicationMicrostation;

        /// <summary>
        /// Параметры и установки
        /// </summary>
        private readonly IProjectSignatureSettings _projectSignatureSettings;

        /// <summary>
        /// Класс для отображения изменений и логгирования
        /// </summary>
        private readonly IMessagingService _messagingService;

        /// <summary>
        /// Проверка состояния папок и файлов
        /// </summary>   
        private readonly IFileSystemOperations _fileSystemOperations;

        /// <summary>
        /// Операции с путями файлов
        /// </summary>
        private readonly IFilePathOperations _filePathOperations;

        /// <summary>
        /// Сервис для добавления и получения данных о конвертируемых пакетах в серверной части
        /// </summary>     
        private readonly SignatureServerServiceFactory _signatureServerServiceFactory;

        /// <summary>
        /// Создать подписи из прикрепленной библиотеки Microstation в формате Jpeg и отправить в базу данных
        /// </summary>
        public async Task<IResultError> SendJpegSignaturesToDataBase(string filePath) =>
            await MicrostationFileOpen(filePath).
            ResultValueOkBindAsync(CreateJpegFromSignature).
            ResultVoidAsync(_ => _applicationMicrostation.DetachLibrary()).
            ResultVoidAsync(_ => _applicationMicrostation.CloseApplication()).
            MapAsync(result => result.ToResult()).
            VoidAsync(ShowErrors);

        /// <summary>
        /// Отправить подписи Microstation в базу данных
        /// </summary>
        public async Task<IResultError> SendMicrostationDataToDatabase(string filePathMicrostation, MicrostationDataType microstationDataType) =>
            await new ResultValue<string>(filePathMicrostation, new ErrorCommon(ErrorConvertingType.FileNotFound,
                                                                                $"Не найден файл данных Microstation {microstationDataType}")).
                  ResultVoid(_ => _messagingService.ShowMessage($"Обработка данных {microstationDataType} Microstation")).
                  ResultValueOkBindAsync(MicrostationDataBaseToZip).
                  ResultValueOkAsync(zip => new MicrostationDataFile("MicrostationSignatureDataBase", zip)).
                  ResultVoidAsyncBind(dataFile => UploadMicrostationDataToDataBase(dataFile, microstationDataType)).
                  MapAsync(result => result.ToResult()).
                  VoidAsync(ShowErrors);

        /// <summary>
        /// Обработка ошибок
        /// </summary>
        private void ShowErrors(IResultError resultError) =>
            resultError.
            Void(_ => _messagingService.ShowMessage("----------------")).
            WhereContinue(result => result.OkStatus,
                okFunc: result => result.
                                  Void(_ => _messagingService.ShowMessage("Обработка данных успешно завершена")),
                badFunc: result => result.
                                   Void(_ => _messagingService.ShowMessage("Обработка ошибок")).
                                   Void(_ => _messagingService.ShowErrors(result.Errors)));
        /// <summary>
        /// Открыть файл Microstation
        /// </summary>
        private IResultValue<IDocumentMicrostation> MicrostationFileOpen(string filePathMicrostation) =>
            new ResultValue<string>(filePathMicrostation, new ErrorCommon(ErrorConvertingType.FileNotFound,
                                                                          "Не задан путь к файлу Microstation")).
            ResultValueContinue(filePath => _filePathOperations.IsFileExist(filePath),
                okFunc: filePath => filePath,
                badFunc: filePath => new ErrorCommon(ErrorConvertingType.FileNotFound, $"Файл {filePath} не существует")).
            ResultVoidOk(filePath => _messagingService.ShowMessage($"Загрузка файла {filePath}")).
            ResultValueOkBind(filePath => _applicationMicrostation.OpenDocument(filePath).ToResultValueFromApplication());

        /// <summary>
        /// Получить подписи и сохранить изображения
        /// </summary>
        private async Task<IResultValue<Unit>> CreateJpegFromSignature(IDocumentMicrostation documentMicrostation) =>
            await GetSignatures().
            ResultValueOkBind(signatures => signatures.
                                            Select(signature => CreateJpegFromCell(documentMicrostation.ModelsMicrostation[0], signature)).
                                            ToResultCollection()).
            ResultVoidAsyncBind(UploadSignaturesToDataBase).
            MapAsync(result => result.ToResult());

        /// <summary>
        /// Получить список имен и подписей
        /// </summary>
        private IResultCollection<ISignatureLibrary> GetSignatures() =>
            _applicationMicrostation.AttachLibrary(_projectSignatureSettings.SignatureMicrostationFileName).
            ToResultCollectionFromApplication().
            ResultVoid(_ => _messagingService.ShowMessage("Загрузка подписей")).
            ResultValueOk(libraryElements => libraryElements.
                                             Select(libraryElement => new SignatureLibrary(libraryElement.Name, 
                                                                                           PersonInformation.GetFromFullName(libraryElement.Description))).
                                             Where(signature => signature.PersonInformation.HasFullInformation).
                                             Cast<ISignatureLibrary>()).
            ToResultCollection();

        /// <summary>
        /// Сохранить изображение элемента ячейки Microstation
        /// </summary>
        private IResultValue<ISignatureFileData> CreateJpegFromCell(IModelMicrostation modelMicrostation, ISignatureLibrary signatureLibraryBase) =>
            _applicationMicrostation.CreateCellElementFromLibrary(signatureLibraryBase.PersonId, new PointMicrostation(0, 0), modelMicrostation).
            ResultVoidOk(cellElement => cellElement.LineWeight = 7).
            ToResultValueFromApplication().
            ResultVoidOk(_ => _messagingService.ShowMessage($"Обработка подписи {signatureLibraryBase.PersonInformation.FullName}")).
            ResultValueOk(cellSignature => ToJpegByte(cellSignature, signatureLibraryBase));

        /// <summary>
        /// Конвертировать ячейку Microstation в Jpeg
        /// </summary>
        private ISignatureFileData ToJpegByte(IElementMicrostation cellSignature, ISignatureLibrary signatureLibraryBase) =>
            GetSignatureFileSavePath(signatureLibraryBase).
            Void(filePath => cellSignature.DrawToEmfFile(GetSignatureFileSavePath(signatureLibraryBase),
                                                         ProjectSignatureSettings.JpegPixelSize.Width,
                                                         ProjectSignatureSettings.JpegPixelSize.Height)).
            Map(filePathEmf => new SignatureFileData(signatureLibraryBase.PersonId, signatureLibraryBase.PersonInformation,
                                                     JpegConverter.ToJpegFromEmf(filePathEmf), false ).
                               Void(_ => _fileSystemOperations.DeleteFile(filePathEmf))).
            Void(_ => cellSignature.Remove());

        /// <summary>
        /// Получить имя для сохранения подписи
        /// </summary>
        private static string GetSignatureFileSavePath(ISignatureLibrary signatureLibraryBase) =>
            ProjectSignatureSettings.SignaturesSaveFolder + Path.DirectorySeparatorChar +
            signatureLibraryBase.PersonId + ".emf";

        /// <summary>
        /// Загрузить подписи в базу
        /// </summary>
        private async Task UploadSignaturesToDataBase(IReadOnlyList<ISignatureFileData> signatureFileData) =>
            await signatureFileData.
            Void(_ => _messagingService.ShowMessage("Удаление подписей в базе")).
            Map(signatures => _signatureServerServiceFactory.UsingServiceRetry(service => service.Operations.DeleteSignatures())).
            MapAsync(_ => ConverterDataFileToDto.SignaturesToDto(signatureFileData)).
            VoidAsync(_ => _messagingService.ShowMessage("Отправка подписей в базу")).
            MapBindAsync(signatures => _signatureServerServiceFactory.UsingServiceRetry(service => service.Operations.UploadSignatures(signatures))).
            VoidAsync(ShowMessage);

        /// <summary>
        /// Запаковать файл базы Microstation и преобразовать в байтовый массив
        /// </summary>
        private Task<IResultValue<byte[]>> MicrostationDataBaseToZip(string filePath) =>
            _fileSystemOperations.FileToByteAndZip(filePath);

        /// <summary>
        /// Загрузить данные Microstation в базу
        /// </summary>
        private async Task UploadMicrostationDataToDataBase(MicrostationDataFile microstationDataFile, MicrostationDataType microstationDataType) =>
            await ConverterMicrostationDataToDto.MicrostationDataFileToDto(microstationDataFile).
            Void(_ => _messagingService.ShowMessage("Отправка данных в базу")).
            Map(dataFile => microstationDataType switch
                {
                    MicrostationDataType.Signature =>  _signatureServerServiceFactory.UsingServiceRetry(service => service.Operations.UploadSignaturesMicrostation(dataFile)),
                    MicrostationDataType.Stamp => _signatureServerServiceFactory.UsingServiceRetry(service => service.Operations.UploadStampsMicrostation(dataFile)),
                    _ => throw new ArgumentOutOfRangeException(nameof(microstationDataType), microstationDataType, @"Не найден тип данных Microstation")
                }).
            VoidAsync(ShowMessage);

        /// <summary>
        /// Отображение статуса отправки файлов
        /// </summary>
        private void ShowMessage(IResultValue<Unit> resultError) =>
            resultError.
            ResultVoidOk(_ => _messagingService.ShowMessage("Данные записаны в базе")).
            ResultVoidBad(_ => _messagingService.ShowMessage("Обнаружены ошибки")).
            ResultVoidBad(_ => _messagingService.ShowErrors(resultError.Errors));
    }
}
