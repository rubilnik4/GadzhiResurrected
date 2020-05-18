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
        public static IResultAppCollection<T> ToResultCollection<T>(this IEnumerable<IResultAppValue<T>> @this, IErrorApplication errorNull = null) =>
            @this != null 
                ? @this.Aggregate((IResultAppCollection<T>)new ResultAppCollection<T>(), (resultCollection, resultValue) =>
                                      resultCollection.ConcatResultValue(resultValue)).
                        WhereBad(result => result.Value.Count > 0, 
                            badFunc: result => errorNull != null 
                                               ? new ResultAppCollection<T>(errorNull) 
                                               : result)
                : throw new ArgumentNullException(nameof(@this));

        /// <summary>
        /// Преобразовать коллекцию ответов в ответ с коллекцией
        /// </summary>
        public static IResultAppCollection<T> ToResultCollection<T>(this IResultAppValue<IEnumerable<T>> @this, IErrorApplication errorNull = null) =>
            @this != null
                ? new ResultAppCollection<T>(@this.Value, @this.Errors, errorNull) 
                : throw new ArgumentNullException(nameof(@this));

        /// <summary>
        /// Преобразовать в другой подтип
        /// </summary>
        public static IResultAppCollection<TBase> Cast<TDerived, TBase>(this IResultAppCollection<TDerived> resultDerived)
            where TDerived : class, TBase =>
            resultDerived != null
                ? new ResultAppCollection<TBase>(resultDerived.Value.Cast<TBase>(), resultDerived.Errors)
                : throw new ArgumentNullException(nameof(resultDerived));
    }
}
