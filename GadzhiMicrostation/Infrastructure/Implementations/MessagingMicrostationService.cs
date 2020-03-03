using GadzhiMicrostation.Helpers.Converters;
using GadzhiMicrostation.Infrastructure.Interfaces;
using GadzhiMicrostation.Models.Enums;
using GadzhiMicrostation.Models.Implementations;
using GadzhiMicrostation.Models.Interfaces;
using System;
using System.Collections.Generic;

namespace GadzhiMicrostation.Infrastructure.Implementations
{
    /// <summary>
    /// Класс для отображения изменений и логгирования Microstation
    /// </summary>
    public class MessagingMicrostationService : IMessagingMicrostationService
    {
        /// <summary>
        /// Модель хранения данных конвертации
        /// </summary>
        private readonly IMicrostationProject _microstationProject;

        /// <summary>
        /// Запись в журнал системных сообщений
        /// </summary>
        private readonly ILoggerMicrostationService _loggerMicrostation;

        public MessagingMicrostationService(IMicrostationProject microstationProject,
                                            ILoggerMicrostationService loggerMicrostation)
        {
            _microstationProject = microstationProject;
            _loggerMicrostation = loggerMicrostation;
        }

        /// <summary>
        /// Отобразить и записать в журнал сообщение
        /// </summary>
        public void ShowAndLogMessage(string message)
        {
            _loggerMicrostation.LogMessage(message);
            ShowMessage(message);
        }

        /// <summary>
        /// Отобразить и добавить в журнал ошибку
        /// </summary>      
        public void ShowAndLogError(ErrorMicrostation errorMicrostation)
        {
            if (errorMicrostation != null)
            {
                _microstationProject.FileDataMicrostation.AddFileConvertErrorType(errorMicrostation.ErrorMicrostationType);
                ShowError(errorMicrostation);
                _loggerMicrostation.LogError(errorMicrostation);
            }
        }

        /// <summary>
        /// Отобразить сообщение
        /// </summary>
        private void ShowMessage(string message) => Console.WriteLine(message);

        /// <summary>
        /// Отобразить сообщение
        /// </summary>
        private void ShowError(ErrorMicrostation errorMicrostation)
        {
            if (errorMicrostation != null)
            {
                string messageText = "Ошибка | " + ConvertErrorTypeToString.ConvertErrorMicrostationTypeToString(errorMicrostation.ErrorMicrostationType);
                if (!String.IsNullOrEmpty(errorMicrostation?.ErrorDescription))
                {
                    messageText += "\n" + errorMicrostation?.ErrorDescription;
                }
                Console.WriteLine(messageText);
            }
        }
    }
}
