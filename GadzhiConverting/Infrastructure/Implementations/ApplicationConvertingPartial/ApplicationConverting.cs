using GadzhiApplicationCommon.Models.Interfaces.ApplicationLibrary.Application;
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
using System.Threading.Tasks;

namespace GadzhiConverting.Infrastructure.Implementations.ApplicationConvertingPartial
{
    /// <summary>
    /// Класс для работы с приложениями конвертации
    /// </summary>
    public partial class ApplicationConverting : IApplicationConverting
    {
        /// <summary>
        /// Модуль конвертации Microstation
        /// </summary>   
        private readonly IApplicationLibrary _applicationMicrostation;

        /// <summary>
        /// Модуль конвертации Word
        /// </summary>   
        private readonly IApplicationLibrary _applicationWord;

        /// <summary>
        /// Проверка состояния папок и файлов
        /// </summary>   
        private readonly IFileSystemOperations _fileSystemOperations;

        /// <summary>
        /// Управление печатью пдф
        /// </summary>
        private readonly IPdfCreatorService _pdfCreatorService;

        public ApplicationConverting(IApplicationLibrary applicationMicrostation, IApplicationLibrary applicationWord,
                                     IFileSystemOperations fileSystemOperations, IPdfCreatorService pdfCreatorService)
        {
            _applicationMicrostation = applicationMicrostation;
            _applicationWord = applicationWord;
            _fileSystemOperations = fileSystemOperations;
            _pdfCreatorService = pdfCreatorService;
        }

        /// <summary>
        /// Выбрать библиотеку конвертации по типу расширения
        /// </summary>        
        public FileExtension GetExportFileExtension(FileExtension fileExtensionMain) =>
            fileExtensionMain switch
            {
                FileExtension.Dgn => FileExtension.Dwg,
                FileExtension.Docx => FileExtension.Docx,
                _ => throw new InvalidEnumArgumentException(nameof(fileExtensionMain), (int)fileExtensionMain, typeof(FileExtension))
            };


        /// <summary>
        /// Выбрать библиотеку конвертации по типу расширения
        /// </summary>        
        private IResultValue<IApplicationLibrary> GetActiveLibraryByExtension(FileExtension fileExtension) =>
            fileExtension switch
            {
                FileExtension.Dgn => new ResultValue<IApplicationLibrary>(_applicationMicrostation),
                FileExtension.Docx => new ResultValue<IApplicationLibrary>(_applicationWord),
                _ => new ErrorCommon(FileConvertErrorType.LibraryNotFound, $"Библиотека конвертации для типа {fileExtension} не найдена").
                     ToResultValue<IApplicationLibrary>()
            };
    }
}
