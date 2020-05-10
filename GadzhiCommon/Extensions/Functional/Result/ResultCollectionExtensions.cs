using System;
using System.Collections.Generic;
using System.Linq;
using GadzhiCommon.Extensions.Collection;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiCommon.Models.Interfaces.Errors;

namespace GadzhiCommon.Extensions.Functional.Result
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
            @this != null
                ? @this.Aggregate((IResultCollection<T>)new ResultCollection<T>(), (resultCollection, resultValue) =>
                                      resultCollection.ConcatResultValue(resultValue)) :
            throw new ArgumentNullException(nameof(@this));

        /// <summary>
        /// Преобразовать коллекцию ответов в ответ с коллекцией
        /// </summary>
        public static IResultCollection<T> ToResultCollection<T>(this IResultValue<IEnumerable<T>> @this) =>
            @this != null 
                ? new ResultCollection<T>(@this.Value.EmptyIfNull(), @this.Errors) 
                : throw new ArgumentNullException(nameof(@this));

        /// <summary>
        /// Преобразовать в другой подтип
        /// </summary>
        public static IResultCollection<TBase> Cast<TDerived, TBase>(this IResultCollection<TDerived> resultDerived)
            where TDerived : class, TBase  =>
            resultDerived != null
                ? new ResultCollection<TBase>(resultDerived.Value, resultDerived.Errors)
                : throw new ArgumentNullException(nameof(resultDerived));
    }
}
