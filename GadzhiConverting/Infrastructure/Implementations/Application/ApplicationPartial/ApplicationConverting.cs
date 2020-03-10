using ConvertingModels.Models.Interfaces.ApplicationLibrary.Application;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiConverting.Infrastructure.Interfaces.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiConverting.Infrastructure.Implementations.Application.ApplicationWordPartial
{
    /// <summary>
    /// Класс для работы с приложением Word
    /// </summary>
    public partial class ApplicationConverting: IApplicationConverting
    {
        /// <summary>
        /// Проверка состояния папок и файлов
        /// </summary>   
        private readonly IApplicationLibrary _applicationLibrary;

        /// <summary>
        /// Проверка состояния папок и файлов
        /// </summary>   
        private readonly IFileSystemOperations _fileSystemOperations;

        /// <summary>
        /// Класс для отображения изменений и логгирования
        /// </summary>
        private readonly IMessagingService _messagingService;

        /// <summary>
        /// Класс обертка для отлова ошибок
        /// </summary> 
        private readonly IExecuteAndCatchErrors _executeAndCatchErrors;

        /// <summary>
        /// Управление печатью пдф
        /// </summary>
        private readonly IPdfCreatorService _pdfCreatorService;

        public ApplicationConverting(IApplicationLibrary applicationLibrary,
                                     IFileSystemOperations fileSystemOperations,
                                     IMessagingService messagingService,
                                     IExecuteAndCatchErrors executeAndCatchErrors,
                                     IPdfCreatorService pdfCreatorService)
        {
            _applicationLibrary = applicationLibrary;
            _fileSystemOperations = fileSystemOperations;
            _messagingService = messagingService;
            _executeAndCatchErrors = executeAndCatchErrors;
            _pdfCreatorService = pdfCreatorService;
        }
    }
}
