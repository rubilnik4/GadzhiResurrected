using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GadzhiApplicationCommon.Models.Enums;

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
        string Description { get; }

        /// <summary>
        /// Преобразовать в ответ
        /// </summary>      
        IResultApplication ToResultApplication();

        /// <summary>
        /// Преобразовать в ответ с вложенным типом
        /// </summary>      
        IResultAppValue<TValue> ToResultApplicationValue<TValue>();
        
        /// <summary>
        /// Преобразовать в ответ с коллекцией
        /// </summary>      
        IResultAppCollection<TValue> ToResultCollection<TValue>();
    }
}
