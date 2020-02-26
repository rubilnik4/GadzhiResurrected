using GadzhiMicrostation.Infrastructure.Interfaces;
using GadzhiMicrostation.Models.Enums;
using GadzhiMicrostation.Models.Implementations;
using GadzhiMicrostation.Models.Interfaces;
using System.Collections.Generic;

namespace GadzhiMicrostation.Infrastructure.Implementations
{
    /// <summary>
    /// Сообщения об ошибках
    /// </summary>
    public class ErrorMessagingMicrostation : IErrorMessagingMicrostation
    {
        /// <summary>
        /// Модель хранения данных конвертации
        /// </summary>
        private readonly IMicrostationProject _microstationProject;

        /// <summary>
        /// Отображение системных сообщений
        /// </summary>
        private readonly ILoggerMicrostation _loggerMicrostation;

        public ErrorMessagingMicrostation(IMicrostationProject microstationProject,
                                          ILoggerMicrostation loggerMicrostation)
        {
            _microstationProject = microstationProject;
            _loggerMicrostation = loggerMicrostation;
        }       

        /// <summary>
        /// Добавить ошибку
        /// </summary>      
        public void AddError(ErrorMicrostation errorMicrostation)
        {
            if (errorMicrostation != null)
            {
                _microstationProject.FileDataMicrostation.AddFileConvertErrorType(errorMicrostation.ErrorMicrostationType);
                _loggerMicrostation.ShowError(errorMicrostation);
            }
        }
    }
}
