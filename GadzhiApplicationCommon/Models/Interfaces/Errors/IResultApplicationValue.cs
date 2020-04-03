using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiApplicationCommon.Models.Interfaces.Errors
{
    /// <summary>
    /// Базовый вариант ответа
    /// </summary>
    public interface IResultApplicationValue<TValue> : IResultApplication
    {
        /// <summary>
        /// Список значений
        /// </summary>
        TValue Value { get; }
    }
}
