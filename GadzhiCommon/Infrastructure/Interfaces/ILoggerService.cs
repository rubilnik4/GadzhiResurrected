using GadzhiCommon.Models.Interfaces.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiCommon.Infrastructure.Interfaces
{
    /// <summary>
    /// Запись в журнал системных сообщений
    /// </summary>    
    public interface ILoggerService
    {
        /// <summary>
        /// Записать ошибку
        /// </summary>        
        void LogError(IErrorConverting errorConverting);

        /// <summary>
        /// Записать сообщение
        /// </summary>
        void LogMessage(string message);
    }
}
