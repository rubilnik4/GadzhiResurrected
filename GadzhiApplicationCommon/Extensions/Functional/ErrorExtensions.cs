using System.Collections.Generic;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Interfaces.Errors;

namespace GadzhiApplicationCommon.Extensions.Functional
{
    /// <summary>
    /// Преобразование внутреннего класса ошибок библиотеки в основной
    /// </summary>
    public static class ErrorExtensions
    {
        /// <summary>
        /// Преобразовать коллекцию ошибок в результирующий ответ с параметром
        /// </summary>       
        public static IResultApplication ToResultApplication(this IEnumerable<IErrorApplication> @this) =>
            new ResultApplication(@this);

        /// <summary>
        /// Преобразовать коллекцию ошибок в результирующий ответ с параметром
        /// </summary>       
        public static IResultAppValue<TValue> ToResultApplicationValue<TValue>(this IEnumerable<IErrorApplication> @this) =>
            new ResultAppValue<TValue>(@this);
    }   
}
