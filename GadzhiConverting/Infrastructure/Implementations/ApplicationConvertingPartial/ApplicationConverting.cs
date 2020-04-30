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
// ReSharper disable All

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

        public ApplicationConverting(IApplicationLibrary applicationMicrostation,
                                     IApplicationLibrary applicationWord,
                                     IFileSystemOperations fileSystemOperations,
                                     IPdfCreatorService pdfCreatorService)
        {
            _applicationMicrostation = applicationMicrostation;
            _applicationWord = applicationWord;
            _fileSystemOperations = fileSystemOperations;
            _pdfCreatorService = pdfCreatorService;
        }

        /// <summary>
        /// Выбрать библиотеку конвертации по типу расширения
        /// </summary>        
        public FileExtention GetExportFileExtension(FileExtention fileExtentionMain) =>
            fileExtentionMain switch
            {
                FileExtention.dgn => FileExtention.dwg,
                FileExtention.docx => FileExtention.docx,
                _ => throw new InvalidEnumArgumentException(nameof(fileExtentionMain), (int)fileExtentionMain, typeof(FileExtention))
            };


        /// <summary>
        /// Выбрать библиотеку конвертации по типу расширения
        /// </summary>        
        private IResultValue<IApplicationLibrary> GetActiveLibraryByExtension(FileExtention fileExtention) =>
            fileExtention switch
            {
                FileExtention.dgn => new ResultValue<IApplicationLibrary>(_applicationMicrostation),
                FileExtention.docx => new ResultValue<IApplicationLibrary>(_applicationWord),
                _ => new ErrorCommon(FileConvertErrorType.LibraryNotFound, $"Библиотека конвертации для типа {fileExtention} не найдена").
                     ToResultValue<IApplicationLibrary>()
            };
    }
}
