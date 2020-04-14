using GadzhiCommon.Models.Implementations.Errors;
using GadzhiCommon.Models.Interfaces.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiCommon.Extentions.Functional
{
    /// <summary>
    /// Методы расширения для ошибок
    /// </summary>
    public static class ErrorExtension
    {
        /// <summary>
        /// Преобразовать коллекцию ошибок в результирующий ответ с параметром
        /// </summary>       
        public static IResult ToResultApplication(this IEnumerable<IErrorCommon> @this) =>
            new Result(@this);

        /// <summary>
        /// Преобразовать коллекцию ошибок в результирующий ответ с параметром
        /// </summary>       
        public static IResultValue<T> ToResultApplicationValue<T>(this IEnumerable<IErrorCommon> @this) =>
            new ResultValue<T>(@this);
    }
}
