using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiCommon.Extentions.Functional;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiCommon.Models.Interfaces.Errors;
using GadzhiConverting.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiConverting.Models.Converters
{
    /// <summary>
    /// Преобразование результирующего ответа модуля конвертации в основной
    /// </summary>
    public static class ResultApplicationConverter
    {
        /// <summary>
        /// Преобразовать результирующий отвеа модуля конвертации в основной
        /// </summary>      
        public static IResultError ToResult(IResultApplication resultApplication) =>
            resultApplication?.
            Map(result => new ResultError(result.Errors.ToErrorsConverting()))
            ?? throw new ArgumentNullException(nameof(resultApplication));

        /// <summary>
        /// Преобразовать результирующий ответ модуля со значением конвертации в основной
        /// </summary>      
        public static GadzhiCommon.Models.Interfaces.Errors.IResultValue<TResult> ToResultValue<TApplication, TResult>(GadzhiApplicationCommon.Models.Interfaces.Errors.IResultValue<TApplication> resultApplicationValue,
                                                                                 Func<TApplication, TResult> converterValue)
        {
            if (resultApplicationValue == null) throw new ArgumentNullException(nameof(resultApplicationValue));
            if (converterValue == null) throw new ArgumentNullException(nameof(converterValue));

            return resultApplicationValue.
            Map(result => new ResultValue<TResult>(converterValue(result.Value), result.Errors.ToErrorsConverting()));
        }
    }
}
