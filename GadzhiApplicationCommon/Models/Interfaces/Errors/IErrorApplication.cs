using GadzhiMicrostation.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiApplicationCommon.Models.Interfaces.Errors
{
    /// <summary>
    /// Ошибка приложения конвертации
    /// </summary>
    public interface IErrorApplication : IEnumerable<IErrorApplication>
    {
        /// <summary>
        /// Тип ошибки при конвертации Microstation
        /// </summary>
        ErrorApplicationType ErrorMicrostationType { get; }

        /// <summary>
        /// Описание ошибки
        /// </summary>
        string ErrorDescription { get; }

        /// <summary>
        /// Преобразовать в ответ
        /// </summary>      
        IResultApplication ToResultApplication();

        /// <summary>
        /// Преобразовать в ответ с вложенным типом
        /// </summary>      
        IResultApplicationValue<TValue> ToResultApplicationValue<TValue>();
    }
}
