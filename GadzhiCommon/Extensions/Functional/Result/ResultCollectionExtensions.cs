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
        public static IResultCollection<T> ToResultCollection<T>(this IEnumerable<IResultValue<T>> @this, IErrorCommon errorNull = null) =>
            @this != null
                ? @this.Aggregate((IResultCollection<T>)new ResultCollection<T>(), (resultCollection, resultValue) =>
                                      resultCollection.ConcatResultValue(resultValue)).
                        WhereBad(result => result.Value.Count > 0,
                            badFunc: result => errorNull != null
                                               ? new ResultCollection<T>(errorNull)
                                               : result)
                : throw new ArgumentNullException(nameof(@this));

        /// <summary>
        /// Преобразовать коллекцию ответов в ответ с коллекцией
        /// </summary>
        public static IResultCollection<T> ToResultCollection<T>(this IEnumerable<IResultCollection<T>> @this, IErrorCommon errorNull = null) =>
            @this != null
                ? @this.Aggregate((IResultCollection<T>)new ResultCollection<T>(), (resultCollectionMain, resultCollection) =>
                                      resultCollectionMain.ConcatResult(resultCollection)).
                        WhereBad(result => result.Value.Count > 0,
                            badFunc: result => errorNull != null
                                               ? new ResultCollection<T>(errorNull)
                                               : result)
                : throw new ArgumentNullException(nameof(@this));

        /// <summary>
        /// Преобразовать коллекцию ответов в ответ с коллекцией
        /// </summary>
        public static IResultCollection<T> ToResultCollection<T>(this IResultValue<IEnumerable<T>> @this, IErrorCommon errorNull = null) =>
            @this != null 
                ? new ResultCollection<T>(@this.OkStatus
                                                 ? @this.Value
                                                 : @this.Value ?? Enumerable.Empty<T>(),
                                          @this.Errors, errorNull) 
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
