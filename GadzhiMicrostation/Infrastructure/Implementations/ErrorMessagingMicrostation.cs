using GadzhiMicrostation.Infrastructure.Interfaces;
using GadzhiMicrostation.Models.Implementations;
using GadzhiMicrostation.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Infrastructure.Implementations
{
    /// <summary>
    /// Сообщения об ошибках
    /// </summary>
    public class ErrorMessagingMicrostation: IErrorMessagingMicrostation
    {
        /// <summary>
        /// Модель хранения данных конвертации
        /// </summary>
        private readonly  IMicrostationProject _microstationProject;

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
        /// Получить список ошибок
        /// </summary>
        public IEnumerable<ErrorMicrostation> ErrorsMicrostation => _microstationProject.ErrorsMicrostation;


        /// <summary>
        /// Добавить ошибку
        /// </summary>      
        public void AddError(ErrorMicrostation errorMicrostation)
        {
            _microstationProject.AddError(errorMicrostation);
            _loggerMicrostation.ShowError(errorMicrostation);

        }
    }
}
