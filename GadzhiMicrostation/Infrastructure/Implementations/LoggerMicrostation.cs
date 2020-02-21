using GadzhiMicrostation.Helpers.Converters;
using GadzhiMicrostation.Infrastructure.Interfaces;
using GadzhiMicrostation.Models.Implementations;
using System;

namespace GadzhiMicrostation.Infrastructure.Implementations
{
    /// <summary>
    /// Отображение системных сообщений
    /// </summary>
    public class LoggerMicrostation : ILoggerMicrostation
    {
        /// <summary>
        /// Отобразить ошибку
        /// </summary>       
        public void ShowError(ErrorMicrostation errorMicrostation)
        {
            if(errorMicrostation != null)
            {
                string messageText = "Ошибка | " +
                                 ConvertErrorTypeToString.ConvertErrorMicrostationTypeToString(errorMicrostation.ErrorMicrostationType) + "\n" +
                                 errorMicrostation.ErrorDescription;

                Console.WriteLine(messageText);
            }           
        }

        /// <summary>
        /// Отобразить сообщение
        /// </summary>
        public void ShowMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
}
