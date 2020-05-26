using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiApplicationCommon.Extensions.Collection
{
    /// <summary>
    /// Методы расширения для перечислений
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Проверка перечисления на null
        /// </summary>      
        public static IEnumerable<T> EmptyIfNullApp<T>(this IEnumerable<T> source) => source ?? Enumerable.Empty<T>();

        /// <summary>
        /// Объединить элемент и инициализировать, если они пустые
        /// </summary>       
        public static IEnumerable<T> UnionNotNullApp<T>(this IEnumerable<T> first, IEnumerable<T> second) =>
            first.EmptyIfNullApp().Union(second.EmptyIfNullApp());
    }
}
