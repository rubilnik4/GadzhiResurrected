using GadzhiMicrostation.Models.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Infrastructure.Interfaces
{
    /// <summary>
    /// Сервис работы с ошибками
    /// </summary>
    public interface IErrorMessagingMicrostation
    {
        /// <summary>
        /// Получить список ошибок
        /// </summary>
        IEnumerable<ErrorMicrostation> ErrorsMicrostation { get; }

        /// <summary>
        /// Добавить ошибку
        /// </summary>      
        void AddError(ErrorMicrostation errorMicrostation);       
    }
}
