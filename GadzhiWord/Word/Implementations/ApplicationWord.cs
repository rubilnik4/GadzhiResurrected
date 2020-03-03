using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiWord.Factory;
using GadzhiWord.Word.Interfaces;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiWord.Word.Implementations
{
    /// <summary>
    /// Класс для работы с приложением Word
    /// </summary>
    public class ApplicationWord: IApplicationWord
    {

        /// <summary>
        /// Класс обертка для отлова ошибок
        /// </summary> 
        private readonly IExecuteAndCatchErrors _executeAndCatchErrors;

        public ApplicationWord(IExecuteAndCatchErrors executeAndCatchErrors)
        {
            _executeAndCatchErrors = executeAndCatchErrors;          
        }

        /// <summary>
        /// Экземпляр приложения
        /// </summary>
        private Application _application;

        /// <summary>
        /// Экземпляр приложения
        /// </summary>
        private Application Application
        {
            get
            {
                if (_application == null)
                {
                    _executeAndCatchErrors.ExecuteAndHandleError(() => _application = WordInstance.Instance(),
                                          applicationCatchMethod:() => new ErrorConverting(FileConvertErrorType.ApplicationNotLoad,
                                                                                       "Ошибка загрузки приложения Word"));
                }
                return _application;
            }
        }

        /// <summary>
        /// Загрузилась ли оболочка Microstation
        /// </summary>
        public bool IsApplicationValid => Application != null;

    }
}
