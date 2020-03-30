using GadzhiApplicationCommon.Models.Interfaces.ApplicationLibrary.Application;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiCommon.Models.Interfaces.Errors;
using GadzhiConverting.Infrastructure.Interfaces;
using GadzhiConverting.Infrastructure.Interfaces.ApplicationConvertingPartial;
using System;
using System.Collections.Generic;
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
        /// Класс обертка для отлова ошибок
        /// </summary> 
        private readonly IExecuteAndCatchErrors _executeAndCatchErrors;

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
                                     IExecuteAndCatchErrors executeAndCatchErrors,
                                     IFileSystemOperations fileSystemOperations,
                                     IPdfCreatorService pdfCreatorService)
        {
            _applicationMicrostation = applicationMicrostation;
            _applicationWord = applicationWord;
            _executeAndCatchErrors = executeAndCatchErrors;
            _fileSystemOperations = fileSystemOperations;
            _pdfCreatorService = pdfCreatorService;
        }

        /// <summary>
        /// Текущий использумый модуль конвертации
        /// </summary>
        private IApplicationLibrary ActiveLibrary { get; set; }

        /// <summary>
        /// Выбрать библиотеку конвертации по типу расширениф
        /// </summary>        
        private IErrorConverting SetActiveLibraryByExtension(FileExtention fileExtention)
        {
            IErrorConverting libraryError = null;

            if (fileExtention == FileExtention.dgn)
            {
                ActiveLibrary = _applicationMicrostation;
            }
            else if (fileExtention == FileExtention.docx)
            {
                ActiveLibrary = _applicationWord;
            }
            else
            {
                libraryError = new ErrorConverting(FileConvertErrorType.LibraryNotFound,
                                                   $"Библиотека конвертации для типа {fileExtention} не найдена");
            }
            return libraryError;
        }
    }
}
