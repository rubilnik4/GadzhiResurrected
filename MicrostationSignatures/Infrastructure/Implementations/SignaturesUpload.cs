using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GadzhiApplicationCommon.Models.Interfaces.ApplicationLibrary.Document;
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
using GadzhiCommon.Functional;
using GadzhiDTOServer.Contracts.FilesConvert;

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
        /// Создать подписи Microstation в базу данных
        /// </summary>
        public async Task<IResultError> SendMicrostationSignaturesToDatabase(string filePathMicrostation) =>
            await new ResultValue<string>(filePathMicrostation, new ErrorCommon(FileConvertErrorType.FileNotFound, "Не найден файл подписей Microstation")).
            ResultVoid(_ => _messagingService.ShowAndLogMessage("Обработка подписей Microstation")).
            ResultValueOkBindAsync(MicrostationDataBaseToZip).
            ResultValueOkAsync(zip => new SignatureLibraryMicrostation("SignatureMicrostation", zip)).
            ResultVoidAsyncBind(UploadSignaturesMicrostationToDataBase).
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
                                  Void(_ => _messagingService.ShowAndLogMessage("Обработка подписей успешно завершена")),
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
        private IResultCollection<SignatureLibrary> GetSignatures() =>
            _applicationMicrostation.AttachLibrary(ProjectSignatureSettings.SignatureMicrostationFileName).
            ToResultCollectionFromApplication().
            ResultVoid(_ => _messagingService.ShowAndLogMessage("Загрузка подписей")).
            ResultValueOk(libraryElements => libraryElements.
                                             Select(libraryElement => new SignatureLibrary(libraryElement.Name, libraryElement.Description))).
            ToResultCollection();

        /// <summary>
        /// Сохранить изображение элемента ячейки Microstation
        /// </summary>
        private IResultValue<SignatureLibrary> CreateJpegFromCell(IModelMicrostation modelMicrostation, SignatureLibrary signatureLibrary) =>
            _applicationMicrostation.CreateCellElementWithoutCheck(signatureLibrary.Id, new PointMicrostation(0, 0), modelMicrostation).
            ToResultValueFromApplication().
            ResultVoidOk(_ => _messagingService.ShowAndLogMessage($"Обработка подписи {signatureLibrary.Fullname}")).
            ResultValueOk(cellSignature => ToJpegByte(cellSignature, signatureLibrary));

        /// <summary>
        /// Конвертировать ячейку Microstation в Jpeg
        /// </summary>
        private SignatureLibrary ToJpegByte(IElementMicrostation cellSignature, SignatureLibrary signatureLibrary) =>
            GetSignatureFileSavePath(signatureLibrary).
            Void(filePath => cellSignature.DrawToEmfFile(GetSignatureFileSavePath(signatureLibrary),
                                                         ProjectSignatureSettings.JpegPixelSize.Width,
                                                         ProjectSignatureSettings.JpegPixelSize.Height)).
            Map(filePathEmf => new SignatureLibrary(signatureLibrary.Id, signatureLibrary.Fullname, JpegConverter.ToJpegFromEmf(filePathEmf)).
                               Void(_ => _fileSystemOperations.DeleteFile(filePathEmf))).
            Void(_ => cellSignature.Remove());

        /// <summary>
        /// Получить имя для сохранения подписи
        /// </summary>
        private static string GetSignatureFileSavePath(SignatureLibrary signatureLibrary) =>
            ProjectSignatureSettings.SignaturesSaveFolder + Path.DirectorySeparatorChar +
            signatureLibrary.Id + ".emf";

        /// <summary>
        /// Загрузить подписи в базу
        /// </summary>
        private async Task UploadSignaturesToDataBase(IReadOnlyList<SignatureLibrary> signaturesLibrary) =>
            await ConverterSignatureToDto.SignaturesToDto(signaturesLibrary).
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
        /// Загрузить подписи Microstation в базу
        /// </summary>
        private async Task UploadSignaturesMicrostationToDataBase(SignatureLibraryMicrostation signatureLibraryMicrostation) =>
            await ConverterSignatureToDto.SignatureMicrostationToDto(signatureLibraryMicrostation).
                  Void(_ => _messagingService.ShowAndLogMessage("Отправка данных в базу")).
                  VoidAsync(signatures => _fileConvertingServerService.Operations.UploadSignaturesMicrostation(signatures)).
                  VoidAsync(_ => _messagingService.ShowAndLogMessage("Данные записаны в базе"));
    }
}
