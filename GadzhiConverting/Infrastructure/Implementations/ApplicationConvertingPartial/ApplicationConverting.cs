﻿using GadzhiApplicationCommon.Models.Interfaces.ApplicationLibrary.Application;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiCommon.Models.Interfaces.Errors;
using GadzhiConverting.Infrastructure.Interfaces;
using GadzhiConverting.Infrastructure.Interfaces.ApplicationConvertingPartial;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using GadzhiApplicationCommon.Models.Interfaces.ApplicationLibrary.Document;
using GadzhiCommon.Infrastructure.Implementations.Logger;
using GadzhiCommon.Infrastructure.Interfaces.Logger;
using GadzhiMicrostation.Microstation.Interfaces.DocumentMicrostationPartial;
using GadzhiPdfPrinting.Infrastructure.Interfaces;
using GadzhiWord.Word.Interfaces.Word;

namespace GadzhiConverting.Infrastructure.Implementations.ApplicationConvertingPartial
{
    /// <summary>
    /// Класс для работы с приложениями конвертации
    /// </summary>
    public partial class ApplicationConverting : IApplicationConverting
    {
        public ApplicationConverting(IApplicationLibrary<IDocumentMicrostation> applicationMicrostation,
                                     IApplicationLibrary<IDocumentWord> applicationWord,
                                     IFileSystemOperations fileSystemOperations, IFilePathOperations filePathOperations,
                                     IPdfCreatorService pdfCreatorService, IMessagingService messagingService)
        {
            _applicationMicrostation = applicationMicrostation;
            _applicationWord = applicationWord;
            _fileSystemOperations = fileSystemOperations;
            _filePathOperations = filePathOperations;
            _pdfCreatorService = pdfCreatorService;
            _messagingService = messagingService;
        }

        /// <summary>
        /// Журнал системных сообщений
        /// </summary>
        private static readonly ILoggerService _loggerService = LoggerFactory.GetFileLogger();

        /// <summary>
        /// Модуль конвертации Microstation
        /// </summary>   
        private readonly IApplicationLibrary<IDocumentMicrostation> _applicationMicrostation;

        /// <summary>
        /// Модуль конвертации Word
        /// </summary>   
        private readonly IApplicationLibrary<IDocumentWord> _applicationWord;

        /// <summary>
        /// Проверка состояния папок и файлов
        /// </summary>   
        private readonly IFileSystemOperations _fileSystemOperations;

        private readonly IFilePathOperations _filePathOperations;

        /// <summary>
        /// Управление печатью пдф
        /// </summary>
        private readonly IPdfCreatorService _pdfCreatorService;

        /// <summary>
        /// Класс для отображения изменений и логгирования
        /// </summary>
        private readonly IMessagingService _messagingService;

        /// <summary>
        /// Выбрать библиотеку конвертации по типу расширения
        /// </summary>        
        public FileExtensionType GetExportFileExtension(FileExtensionType fileExtensionTypeMain) =>
            fileExtensionTypeMain switch
            {
                FileExtensionType.Dgn => FileExtensionType.Dwg,
                FileExtensionType.Doc => FileExtensionType.Xlsx,
                FileExtensionType.Docx => FileExtensionType.Xlsx,
                _ => throw new InvalidEnumArgumentException(nameof(fileExtensionTypeMain), (int)fileExtensionTypeMain, typeof(FileExtensionType))
            };

        /// <summary>
        /// Закрыть приложения
        /// </summary>
        public void CloseApplications()
        {
            _applicationMicrostation.CloseApplication();
            _applicationWord.CloseApplication();
        }

        /// <summary>
        /// Выбрать библиотеку конвертации по типу расширения
        /// </summary>        
        private IResultValue<IApplicationLibrary<IDocumentLibrary>> GetActiveLibraryByExtension(FileExtensionType fileExtensionType) =>
            fileExtensionType switch
            {
                FileExtensionType.Dgn => new ResultValue<IApplicationLibrary<IDocumentLibrary>>(_applicationMicrostation),
                FileExtensionType.Doc => new ResultValue<IApplicationLibrary<IDocumentLibrary>>(_applicationWord),
                FileExtensionType.Docx => new ResultValue<IApplicationLibrary<IDocumentLibrary>>(_applicationWord),
                _ => new ErrorCommon(ErrorConvertingType.LibraryNotFound, $"Библиотека конвертации для типа {fileExtensionType} не найдена").
                     ToResultValue<IApplicationLibrary<IDocumentLibrary>>()
            };
    }
}
