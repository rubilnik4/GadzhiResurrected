using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiCommon.Models.Interfaces.Errors
{
    /// <summary>
    /// Базовый вариант ответа
    /// </summary>
    public interface IResultConvertingValue<TValue>: IResultConverting
    {
        /// <summary>
        /// Список значений
        /// </summary>
        TValue ValueConverting { get; }
    }
}
