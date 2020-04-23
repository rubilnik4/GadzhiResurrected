using GadzhiCommon.Models.Implementations.Errors;
using GadzhiCommon.Models.Interfaces.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiCommon.Extentions.Functional.Result
{
    /// <summary>
    /// Преобразование в ответ с коллекцией
    /// </summary>
    public static class ResultCollectionExtensions
    {
        /// <summary>
        /// Преобразовать коллекцию ответов в ответ с коллекцией
        /// </summary>
        public static IResultCollection<T> ToResultCollection<T>(this IEnumerable<IResultValue<T>> @this) =>
        @this == null ?
        @this.Aggregate((IResultCollection<T>)new ResultCollection<T>(), (resultCollection, resultValue) =>
                        resultCollection.ConcatResultValue(resultValue)) :
        throw new ArgumentNullException(nameof(@this));

        /// <summary>
        /// Преобразовать коллекцию ответов в ответ с коллекцией
        /// </summary>
        public static IResultCollection<T> ToResultCollection<T>(this IResultValue<IEnumerable<T>> @this) =>
        @this != null ?
        new ResultCollection<T>(@this.Value, @this.Errors) :
        throw new ArgumentNullException(nameof(@this));
    }
}
