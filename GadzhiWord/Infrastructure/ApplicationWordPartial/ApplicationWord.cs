using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiWord.Factory;
using GadzhiWord.Infrastructure.Interfaces;
using GadzhiWord.Models.Interfaces;
using GadzhiWord.Word.Implementations.DocumentWordPartial;
using GadzhiWord.Word.Interfaces.ApplicationWordPartial;
using GadzhiWord.Word.Interfaces.DocumentWordPartial;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiWord.Infrastructure.Implementations.ApplicationWordPartial
{
    /// <summary>
    /// Класс для работы с приложением Word
    /// </summary>
    public partial class ApplicationWord: IApplicationWord
    {
        /// <summary>
        /// Модель хранения данных конвертации Word
        /// </summary>
        private readonly IWordProject _wordProject;

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

        public ApplicationWord(IWordProject wordProject,
                               IFileSystemOperations fileSystemOperations,
                               IMessagingService messagingService,
                               IExecuteAndCatchErrors executeAndCatchErrors,
                               IPdfCreatorService pdfCreatorService)
        {
            _wordProject = wordProject;
            _fileSystemOperations = fileSystemOperations;
            _messagingService = messagingService;
            _executeAndCatchErrors = executeAndCatchErrors;
            _pdfCreatorService = pdfCreatorService;
        }
    }
}
