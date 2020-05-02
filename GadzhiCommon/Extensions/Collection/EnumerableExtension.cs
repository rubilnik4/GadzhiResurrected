using System.Collections.Generic;
using System.Linq;

namespace GadzhiCommon.Extensions.Collection
{
    /// <summary>
    /// Методы расширения для перечислений
    /// </summary>
   public static class EnumerableExtension
    {
        /// <summary>
        /// Проверка перечисления на null
        /// </summary>      
        public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> source) => source ?? Enumerable.Empty<T>();

        /// <summary>
        /// Объединить элемент и инициализировать, если они пустые
        /// </summary>       
        public static IEnumerable<T> UnionNotNull<T>(this IEnumerable<T> first, IEnumerable<T> second) =>
            first.EmptyIfNull().Union(second.EmptyIfNull());
    }
}
