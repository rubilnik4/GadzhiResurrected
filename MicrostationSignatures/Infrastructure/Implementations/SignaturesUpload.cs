﻿using System;
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
using MicrostationSignatures.Infrastructure.Interfaces;
using GadzhiConverting.Extensions;
using MicrostationSignatures.Models.Implementations;
using GadzhiApplicationCommon.Extensions.Functional.Result;
using GadzhiMicrostation.Microstation.Interfaces.DocumentMicrostationPartial;
using GadzhiMicrostation.Models.Implementations.Coordinates;
using GadzhiMicrostation.Microstation.Interfaces;
using System.IO;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using ChannelAdam.ServiceModel;
using GadzhiApplicationCommon.Models.Implementation.LibraryData;
using GadzhiCommon.Models.Implementations.Functional;
using GadzhiCommon.Models.Implementations.LibraryData;
using GadzhiCommon.Models.Interfaces.LibraryData;
using GadzhiConverting.Infrastructure.Implementations.Converters;
using GadzhiDTOBase.Infrastructure.Implementations.Converters;
using GadzhiDTOServer.Contracts.FilesConvert;
using MicrostationSignatures.Models.Enums;

namespace MicrostationSignatures.Infrastructure.Implementations
{
    /// <summary>
    /// Преобразование подписей Microstation в Jpeg
    /// </summary>
    public class SignaturesUpload : ISignaturesToJpeg
    {
        /// <summary>
        /// Модуль конвертации Microstation
        /// </summary>   
        private readonly IApplicationMicrostation _applicationMicrostation;

        /// <summary>
        /// Класс для отображения изменений и логгирования
        /// </summary>
        private readonly IMessagingService _messagingService;

        /// <summary>
        /// Проверка состояния папок и файлов
        /// </summary>   
        private readonly IFileSystemOperations _fileSystemOperations;

        /// <summary>
        /// Сервис для добавления и получения данных о конвертируемых пакетах в серверной части
        /// </summary>     
        private readonly IServiceConsumer<IFileConvertingServerService> _fileConvertingServerService;

        public SignaturesUpload(IApplicationMicrostation applicationMicrostation, IMessagingService messagingService,
                                IFileSystemOperations fileSystemOperations,
                                IServiceConsumer<IFileConvertingServerService> fileConvertingServerService)
        {
            _applicationMicrostation = applicationMicrostation ?? throw new ArgumentNullException(nameof(applicationMicrostation));
            _messagingService = messagingService ?? throw new ArgumentNullException(nameof(messagingService));
            _fileSystemOperations = fileSystemOperations ?? throw new ArgumentNullException(nameof(fileSystemOperations));
            _fileConvertingServerService = fileConvertingServerService ?? throw new ArgumentNullException(nameof(fileConvertingServerService));
        }

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
            await new ResultValue<string>(filePathMicrostation, new ErrorCommon(FileConvertErrorType.FileNotFound,
                                                                                $"Не найден файл данных Microstation {microstationDataType}")).
                  ResultVoid(_ => _messagingService.ShowAndLogMessage($"Обработка данных {microstationDataType} Microstation")).
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
            Void(_ => _messagingService.ShowAndLogMessage("----------------")).
            WhereContinue(result => result.OkStatus,
                okFunc: result => result.
                                  Void(_ => _messagingService.ShowAndLogMessage("Обработка данных успешно завершена")),
                badFunc: result => result.
                                   Void(_ => _messagingService.ShowAndLogMessage("Обработка ошибок")).
                                   Void(_ => _messagingService.ShowAndLogErrors(result.Errors)));
        /// <summary>
        /// Открыть файл Microstation
        /// </summary>
        private IResultValue<IDocumentMicrostation> MicrostationFileOpen(string filePathMicrostation) =>
            new ResultValue<string>(filePathMicrostation, new ErrorCommon(FileConvertErrorType.FileNotFound,
                                                                          "Не задан путь к файлу Microstation")).
            ResultValueContinue(filePath => _fileSystemOperations.IsFileExist(filePath),
                okFunc: filePath => filePath,
                badFunc: filePath => new ErrorCommon(FileConvertErrorType.FileNotFound, $"Файл {filePath} не существует")).
            ResultVoidOk(filePath => _messagingService.ShowAndLogMessage($"Загрузка файла {filePath}")).
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
            _applicationMicrostation.AttachLibrary(ProjectSignatureSettings.SignatureMicrostationFileName).
            ToResultCollectionFromApplication().
            ResultVoid(_ => _messagingService.ShowAndLogMessage("Загрузка подписей")).
            ResultValueOk(libraryElements => libraryElements.
                                             Select(libraryElement => new SignatureLibrary(libraryElement.Name, 
                                                                                           PersonInformation.GetFromFullName(libraryElement.Description))).
                                             Where(signature => signature.PersonInformation.HasFullInformation).
                                             Cast<ISignatureLibrary>()).
            ToResultCollection();

        /// <summary>
        /// Сохранить изображение элемента ячейки Microstation
        /// </summary>
        private IResultValue<ISignatureFileData> CreateJpegFromCell(IModelMicrostation modelMicrostation, ISignatureLibrary signatureLibrary) =>
            _applicationMicrostation.CreateCellElementFromLibrary(signatureLibrary.PersonId, new PointMicrostation(0, 0), modelMicrostation).
            ResultVoidOk(cellElement => cellElement.LineWeight = 7).
            ToResultValueFromApplication().
            ResultVoidOk(_ => _messagingService.ShowAndLogMessage($"Обработка подписи {signatureLibrary.PersonInformation.FullName}")).
            ResultValueOk(cellSignature => ToJpegByte(cellSignature, signatureLibrary));

        /// <summary>
        /// Конвертировать ячейку Microstation в Jpeg
        /// </summary>
        private ISignatureFileData ToJpegByte(IElementMicrostation cellSignature, ISignatureLibrary signatureLibrary) =>
            GetSignatureFileSavePath(signatureLibrary).
            Void(filePath => cellSignature.DrawToEmfFile(GetSignatureFileSavePath(signatureLibrary),
                                                         ProjectSignatureSettings.JpegPixelSize.Width,
                                                         ProjectSignatureSettings.JpegPixelSize.Height)).
            Map(filePathEmf => new SignatureFileData(signatureLibrary.PersonId, signatureLibrary.PersonInformation,
                                                     JpegConverter.ToJpegFromEmf(filePathEmf), false ).
                               Void(_ => _fileSystemOperations.DeleteFile(filePathEmf))).
            Void(_ => cellSignature.Remove());

        /// <summary>
        /// Получить имя для сохранения подписи
        /// </summary>
        private static string GetSignatureFileSavePath(ISignatureLibrary signatureLibrary) =>
            ProjectSignatureSettings.SignaturesSaveFolder + Path.DirectorySeparatorChar +
            signatureLibrary.PersonId + ".emf";

        /// <summary>
        /// Загрузить подписи в базу
        /// </summary>
        private async Task UploadSignaturesToDataBase(IReadOnlyList<ISignatureFileData> signatureFileData) =>
            await ConverterDataFileToDto.SignaturesToDto(signatureFileData).
            Void(_ => _messagingService.ShowAndLogMessage("Отправка данных в базу")).
            VoidAsync(signatures => _fileConvertingServerService.Operations.UploadSignatures(signatures)).
            VoidAsync(_ => _messagingService.ShowAndLogMessage("Данные записаны в базе"));

        /// <summary>
        /// Запаковать файл базы Microstation и преобразовать в байтовый массив
        /// </summary>
        private Task<IResultValue<byte[]>> MicrostationDataBaseToZip(string filePath) =>
            _fileSystemOperations.FileToByteAndZip(filePath).
             WhereContinueAsync(successAndZip => successAndZip.Success,
                                okFunc: successAndZip => (IResultValue<byte[]>)new ResultValue<byte[]>(successAndZip.Zip),
                                badFunc: successAndZip => new ResultValue<byte[]>(new ErrorCommon(FileConvertErrorType.IncorrectDataSource,
                                                                                                  "Невозможно преобразовать файл в формат zip")));

        /// <summary>
        /// Загрузить данные Microstation в базу
        /// </summary>
        private async Task UploadMicrostationDataToDataBase(MicrostationDataFile microstationDataFile, MicrostationDataType microstationDataType) =>
            await ConverterMicrostationDataToDto.MicrostationDataFileToDto(microstationDataFile).
                  Void(_ => _messagingService.ShowAndLogMessage("Отправка данных в базу")).
                  VoidAsync(dataFile => microstationDataType switch
                  {
                      MicrostationDataType.Signature => _fileConvertingServerService.Operations.UploadSignaturesMicrostation(dataFile),
                      MicrostationDataType.Stamp => _fileConvertingServerService.Operations.UploadStampsMicrostation(dataFile),
                      _ => throw new ArgumentOutOfRangeException(nameof(microstationDataType), microstationDataType, @"Не найден тип данных Microstation")
                  }).
                  VoidAsync(_ => _messagingService.ShowAndLogMessage("Данные записаны в базе"));
    }
}
