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
        /// ПРоверка перечесления на null
        /// </summary>      
        public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> source) => source ?? Enumerable.Empty<T>();       
    }
}
