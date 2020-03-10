using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiCommon.Models.Interfaces.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiConverting.Infrastructure.Implementations
{
    /// <summary>
    /// Запись в журнал системных сообщений
    /// </summary>
    public class LoggerService: ILoggerService
    {
        /// <summary>
        /// Записать ошибку
        /// </summary>       
        public void LogError(IErrorConverting errorConverting)
        {

        }

        /// <summary>
        /// Записать сообщение
        /// </summary>
        public void LogMessage(string message)
        {

        }
    }
}
