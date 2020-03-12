using GadzhiMicrostation.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiApplicationCommon.Models.Interfaces
{
    /// <summary>
    /// Ошибка приложения конвертации
    /// </summary>
    public interface IErrorApplication
    {
        /// <summary>
        /// Тип ошибки при конвертации Microstation
        /// </summary>
        ErrorApplicationType ErrorMicrostationType { get; }

        /// <summary>
        /// Описание ошибки
        /// </summary>
        string ErrorDescription { get; }
    }
}
