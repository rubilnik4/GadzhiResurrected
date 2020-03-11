using ConvertingModels.Models.Interfaces.ApplicationLibrary.Application;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiConverting.Infrastructure.Interfaces.Application;
using GadzhiConverting.Infrastructure.Interfaces.Application.ApplicationPartial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiConverting.Infrastructure.Implementations.Application.ApplicationPartial
{
    /// <summary>
    /// Класс для работы с приложениями конвертации
    /// </summary>
    public partial class ApplicationConverting: IApplicationConverting
    {
        /// <summary>
        /// Проверка состояния папок и файлов
        /// </summary>   
        private readonly IApplicationLibrary _applicationLibrary;

        /// <summary>
        /// Класс обертка для отлова ошибок
        /// </summary> 
        private readonly IExecuteAndCatchErrors _executeAndCatchErrors;

        /// <summary>
        /// Управление печатью пдф
        /// </summary>
        private readonly IPdfCreatorService _pdfCreatorService;

        public ApplicationConverting(IApplicationLibrary applicationLibrary,
                                     IExecuteAndCatchErrors executeAndCatchErrors,
                                     IPdfCreatorService pdfCreatorService)
        {
            _applicationLibrary = applicationLibrary;                  
            _executeAndCatchErrors = executeAndCatchErrors;
            _pdfCreatorService = pdfCreatorService;
        }
    }
}
