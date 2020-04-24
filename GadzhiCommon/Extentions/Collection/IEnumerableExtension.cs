using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiCommon.Extentions.Collection
{
    /// <summary>
    /// Методы расширения для перечислений
    /// </summary>
   public static class IEnumerableExtension
    {
        /// <summary>
        /// Проверка перечесления на null
        /// </summary>      
        public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> source) => source ?? Enumerable.Empty<T>();

        /// <summary>
        /// Объединить элемент и инициализировать, если они пустые
        /// </summary>       
        public static IEnumerable<T> UnionNotNull<T>(this IEnumerable<T> first, IEnumerable<T> second) =>
            first.EmptyIfNull().Union(second.EmptyIfNull());
    }
}
