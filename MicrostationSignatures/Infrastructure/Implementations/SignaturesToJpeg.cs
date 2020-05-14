using System;
using System.Collections.Generic;
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

namespace MicrostationSignatures.Infrastructure.Implementations
{
    /// <summary>
    /// Преобразование подписей Microstation в Jpeg
    /// </summary>
    public class SignaturesToJpeg: ISignaturesToJpeg
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
            MicrostationFileOpen(filePath);

        /// <summary>
        /// Открыть файл Microstation
        /// </summary>
        private IResultValue<IDocumentLibrary> MicrostationFileOpen(string filePathMicrostation) =>
            new ResultValue<string>(filePathMicrostation, new ErrorCommon(FileConvertErrorType.FileNotFound,
                                                                          "Не задан путь к файлу Microstation")).
            ResultValueContinue(filePath => _fileSystemOperations.IsFileExist(filePath),
                okFunc: filePath => filePath,
                badFunc: filePath => new ErrorCommon(FileConvertErrorType.FileNotFound, $"Файл {filePath} не существует")).
            ResultVoidOk(filePath => _messagingService.ShowAndLogMessage($"Загрузка файла {filePath}")).
            ResultValueOkBind(filePath => _applicationMicrostation.OpenDocument(filePath).ToResultValueFromApplication());

        private void GetSignatures() =>
            _applicationMicrostation.AttachLibrary(ProjectSignatureSettings.SignatureMicrostationFileName);
    }
}
