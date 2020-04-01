using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiApplicationCommon.Extensions.Functional
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

        /// <summary>
        /// Условие возвращающее объект или null 
        /// </summary>      
        public static T WhereMap<T>(this T @this, Func<T, bool> predicate) =>
            predicate != null ?
                predicate(@this) ?
                @this :
            default:
            throw new ArgumentNullException(nameof(predicate));
    }
}
