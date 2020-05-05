using System;
using System.Collections.Generic;
using System.Linq;
using GadzhiApplicationCommon.Extensions.Collection;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Interfaces.Errors;

namespace GadzhiApplicationCommon.Extensions.Functional.Result
{
    /// <summary>
    /// Преобразование в ответ с коллекцией
    /// </summary>
    public static class ResultCollectionExtensions
    {
        /// <summary>
        /// Преобразовать коллекцию ответов в ответ с коллекцией
        /// </summary>
        public static IResultAppCollection<T> ToResultCollection<T>(this IEnumerable<IResultAppValue<T>> @this) =>
            @this != null ?
            @this.Aggregate((IResultAppCollection<T>)new ResultAppCollection<T>(), (resultCollection, resultValue) =>
                            resultCollection.ConcatResultValue(resultValue)) :
            throw new ArgumentNullException(nameof(@this));

        /// <summary>
        /// Преобразовать коллекцию ответов в ответ с коллекцией
        /// </summary>
        public static IResultAppCollection<T> ToResultCollection<T>(this IResultAppValue<IEnumerable<T>> @this) =>
            @this != null ?
            new ResultAppCollection<T>(@this.Value.EmptyIfNullApp(), @this.Errors) :
            throw new ArgumentNullException(nameof(@this));
    }
}
