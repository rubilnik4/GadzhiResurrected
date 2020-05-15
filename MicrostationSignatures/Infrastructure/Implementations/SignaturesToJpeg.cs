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
using System.Runtime.CompilerServices;

namespace MicrostationSignatures.Infrastructure.Implementations
{
    /// <summary>
    /// Преобразование подписей Microstation в Jpeg
    /// </summary>
    public class SignaturesToJpeg : ISignaturesToJpeg
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

        public SignaturesToJpeg(IApplicationMicrostation applicationMicrostation, IMessagingService messagingService,
                                IFileSystemOperations fileSystemOperations)
        {
            _applicationMicrostation = applicationMicrostation ?? throw new ArgumentNullException(nameof(applicationMicrostation));
            _messagingService = messagingService ?? throw new ArgumentNullException(nameof(messagingService));
            _fileSystemOperations = fileSystemOperations ?? throw new ArgumentNullException(nameof(fileSystemOperations));
        }

        /// <summary>
        /// Создать подписи из прикрепленной библиотеки Microstation в формате Jpeg
        /// </summary>
        public void CreateJpegSignatures(string filePath) =>
            MicrostationFileOpen(filePath).
            ResultValueOkBind(CreateJpegFromSignature).
            ResultVoid(_ => _applicationMicrostation.DetachLibrary()).
            ResultVoid(_ => _applicationMicrostation.CloseApplication()).
            Void(result => _messagingService.ShowAndLogMessage("----------------")).
            Void(result => _messagingService.ShowAndLogMessage("Обработка ошибок")).
            Void(result => _messagingService.ShowAndLogErrors(result.Errors));

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
        private IResultError CreateJpegFromSignature(IDocumentMicrostation documentMicrostation) =>
            GetSignatures().
            ResultValueOkBind(signatures => signatures.
                                            Select(signature => CreateJpegFromCell(documentMicrostation.ModelsMicrostation[0], signature)).
                                            ToResultCollection()).
            ToResult();

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
        private IResultError CreateJpegFromCell(IModelMicrostation modelMicrostation, SignatureLibrary signatureLibrary) =>
            _applicationMicrostation.CreateCellElementWithoutCheck(signatureLibrary.Id, new PointMicrostation(0, 0), modelMicrostation).
            ToResultValueFromApplication().
            ResultVoidOk(_ => _messagingService.ShowAndLogMessage($"Обработка подписи {signatureLibrary.Fullname}")).
            ResultVoidOk(cellSignature => DrawToJpeg(cellSignature, signatureLibrary)).
            ResultVoidOk(cellSignature => cellSignature.Remove()).
            ToResult();

        private void DrawToJpeg(IElementMicrostation cellSignature, SignatureLibrary signatureLibrary) =>
            GetSignatureFileSavePath(signatureLibrary).
            Void(filePath => cellSignature.DrawToEmfFile(GetSignatureFileSavePath(signatureLibrary),
                                                         ProjectSignatureSettings.JpegPixelSize.Width,
                                                         ProjectSignatureSettings.JpegPixelSize.Height)).
            Void(JpegConverter.ToJpegFromEmf).
            Void(filePath => _fileSystemOperations.DeleteFile(filePath));

        /// <summary>
        /// Получить имя для сохранения подписи
        /// </summary>
        private static string GetSignatureFileSavePath(SignatureLibrary signatureLibrary) =>
            ProjectSignatureSettings.SignaturesSaveFolder + Path.DirectorySeparatorChar +
            signatureLibrary.Id + ".emf";
    }
}
