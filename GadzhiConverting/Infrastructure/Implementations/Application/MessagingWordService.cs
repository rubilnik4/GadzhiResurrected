using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiCommon.Models.Interfaces.Errors;
using GadzhiConverting.Infrastructure.Implementations;
using GadzhiWord.Infrastructure.Interfaces;
using GadzhiWord.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiConverting.Infrastructure.Implementations.Application
{
    /// <summary>
    /// 
    /// </summary>
    public class MessagingWordService : MessagingService
    {
        /// <summary>
        /// Модель хранения данных конвертации Word
        /// </summary>
        private readonly IWordProject _wordProject;
       
        public MessagingWordService(IWordProject wordProject, ILoggerService loggerService)
            : base(loggerService)
        {
            _wordProject = wordProject;
        }

        /// <summary>
        /// Отобразить ошибку. Внести в модель ошибок
        /// </summary>        
        public override void ShowAndLogError(IErrorConverting errorConverting)           
        {
            if (errorConverting != null)
            {
                base.ShowAndLogError(errorConverting);
                _wordProject.FileDataServerWord.AddFileConvertErrorType(errorConverting.FileConvertErrorType);
            }
        }    
    }
}
