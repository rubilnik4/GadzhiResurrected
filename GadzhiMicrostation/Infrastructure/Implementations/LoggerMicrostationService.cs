using GadzhiMicrostation.Helpers.Converters;
using GadzhiMicrostation.Infrastructure.Interfaces;
using GadzhiMicrostation.Models.Implementations;
using System;

namespace GadzhiMicrostation.Infrastructure.Implementations
{
    /// <summary>
    /// Запись в журнал системных сообщений
    /// </summary>
    public class LoggerMicrostationService : ILoggerMicrostationService
    {
        /// <summary>
        /// Записать ошибку
        /// </summary>       
        public void LogError(ErrorMicrostation errorMicrostation)
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
