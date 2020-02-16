using GadzhiMicrostation.Models.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Infrastructure.Interfaces
{
    /// <summary>
    /// Отображение системных сообщений
    /// </summary>
    public interface ILoggerMicrostation
    {
        /// <summary>
        /// Отобразить ошибку
        /// </summary>        
        void ShowError(ErrorMicrostation errorMicrostation);

        /// <summary>
        /// Отобразить сообщение
        /// </summary>
        void ShowMessage(string message); 
    }
}
