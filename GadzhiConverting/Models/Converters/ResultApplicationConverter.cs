using System;
using System.Linq;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiCommon.Extensions.Functional;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiCommon.Models.Interfaces.Errors;
using GadzhiConverting.Extensions;

namespace GadzhiConverting.Models.Converters
{
    /// <summary>
    /// Преобразование результирующего ответа модуля конвертации в основной
    /// </summary>
    public static class ResultApplicationConverter
    {
        /// <summary>
        /// Преобразовать результирующий ответа модуля конвертации в основной
        /// </summary>      
        public static IResultError ToResult(IResultApplication resultApplication) =>
            resultApplication?.
            Map(result => new ResultError(result.Errors.ToErrorsConverting()))
            ?? throw new ArgumentNullException(nameof(resultApplication));

        /// <summary>
        /// Преобразовать результирующий ответ модуля со значением конвертации в основной
        /// </summary>      
        public static IResultValue<TValue> ToResultValue<TValue>(IResultAppValue<TValue> resultApplicationValue) =>
            resultApplicationValue?.
            Map(result => (resultApplicationValue.OkStatus) 
                            ? new ResultValue<TValue>(result.Value) 
                            : new ResultValue<TValue>(result.Errors.ToErrorsConverting()))
            ?? throw new ArgumentNullException(nameof(resultApplicationValue));

        /// <summary>
        /// Преобразовать результирующий ответ модуля со значением конвертации из основного
        /// </summary>      
        public static IResultAppValue<TValue> ToResultAppValue<TValue>(IResultValue<TValue> resultValue) =>
            resultValue?.
            Map(result => (resultValue.OkStatus)
                            ? new ResultAppValue<TValue>(result.Value)
                            : new ResultAppValue<TValue>(result.Errors.ToErrorsApplication()))
            ?? throw new ArgumentNullException(nameof(resultValue));

        /// <summary>
        /// Преобразовать результирующий ответ модуля конвертации с коллекцией в основной
        /// </summary>      
        public static IResultCollection<TValue> ToResultCollection<TValue>(IResultAppCollection<TValue> resultApplicationCollection) =>
            resultApplicationCollection?.
            Map(result => new ResultCollection<TValue>(result.Value.Select(value => value), 
                                                       result.Errors.ToErrorsConverting()))
            ?? throw new ArgumentNullException(nameof(resultApplicationCollection));

        /// <summary>
        /// Преобразовать результирующий ответ модуля конвертации с коллекцией из основного
        /// </summary>      
        public static IResultAppCollection<TValue> ToResultAppCollection<TValue>(IResultCollection<TValue> resultCollection) =>
            resultCollection?.
            Map(result => new ResultAppCollection<TValue>(result.Value.Select(value => value),
                                                          result.Errors.ToErrorsApplication()))
            ?? throw new ArgumentNullException(nameof(resultCollection));
    }
}
