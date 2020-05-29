using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace GadzhiApplicationCommon.Extensions.Collection
{
    /// <summary>
    /// Методы расширения для списков
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// Получить последний элемент списка
        /// </summary>
        public static T GetLast<T>(this IList<T> @this) =>
            (@this?.Count > 0)
                ? @this[@this.Count - 1]
                : default;
    }
}