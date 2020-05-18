using System;
using System.Linq;
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
        public static IResultValue<TValue> ToResultValue<TValue>(IResultAppValue<TValue> resultApplicationValue)
        {
            if (resultApplicationValue == null) throw new ArgumentNullException(nameof(resultApplicationValue));

            return resultApplicationValue.
            Map(result => new ResultValue<TValue>(result.Value, result.Errors.ToErrorsConverting()));
        }

        /// <summary>
        /// Преобразовать результирующий ответ модуля конвертации с коллекцией в основной
        /// </summary>      
        public static IResultCollection<TValue> ToResultCollection<TValue>(IResultAppCollection<TValue> resultApplicationCollection)
        {
            if (resultApplicationCollection == null) throw new ArgumentNullException(nameof(resultApplicationCollection));

            return resultApplicationCollection.
            Map(result => new ResultCollection<TValue>(result.Value.Select(value => value), 
                                                       result.Errors.ToErrorsConverting()));
        }
    }
}
