using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiApplicationCommon.Extensions.Functional
{
    /// <summary>
    /// Методы расширения для ошибок
    /// </summary>
    public static class ErrorExtension
    {
        /// <summary>
        /// Преобразовать коллекцию ошибок в результирующий ответ с параметром
        /// </summary>       
        public static IResultApplication ToResultApplication(this IEnumerable<IErrorApplication> @this) =>
            new ResultApplication(@this);

        /// <summary>
        /// Преобразовать коллекцию ошибок в результирующий ответ с параметром
        /// </summary>       
        public static IResultApplicationValue<T> ToResultApplicationValue<T>(this IEnumerable<IErrorApplication> @this) =>
            new ResultApplicationValue<T>(@this);
    }
}
