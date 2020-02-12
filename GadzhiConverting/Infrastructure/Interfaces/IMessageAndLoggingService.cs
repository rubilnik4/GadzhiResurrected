using GadzhiConverting.Models.Implementations;
using GadzhiConverting.Models.Implementations.ReactiveSubjects;
using GadzhiConverting.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiConverting.Infrastructure.Interfaces 
{
    /// <summary>
    /// Класс для отображения изменений и логгирования
    /// </summary>
    public interface IMessageAndLoggingService
    {
        /// <summary>
        /// Отобразить ошибку
        /// </summary>        
        void ShowError(ErrorTypeConverting errorTypeConverting);

        /// <summary>
        /// </summary>        
        /// </summary>        
        void ShowMessage(string message);
    }
}
