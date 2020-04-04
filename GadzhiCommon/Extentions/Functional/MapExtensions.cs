using GadzhiCommon.Models.Implementations.Errors;
using GadzhiCommon.Models.Interfaces.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiCommon.Extentions.Functional
{
    /// <summary>
    /// Методы расширения для преобразования типов
    /// </summary>
    public static class MapExtensions
    {
        /// <summary>
        /// Преобразование типов с помощью функции
        /// </summary>       
        public static TResult Map<TSource, TResult>(this TSource @this, Func<TSource, TResult> func) =>
            func != null ?
            func(@this) :
            throw new ArgumentNullException(nameof(func));
    }
}
