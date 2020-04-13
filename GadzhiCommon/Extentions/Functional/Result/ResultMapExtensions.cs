using GadzhiCommon.Models.Implementations.Errors;
using GadzhiCommon.Models.Interfaces.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiCommon.Extentions.Functional
{
    /// <summary>
    /// Преобразование внутреннего типа результирующего ответа
    /// </summary>
    public static class ResultMapExtensions
    {
        /// <summary>
        /// Преобразовать внутренний тип результирующего ответа
        /// </summary>
        public static IResultValue<TValueOut> ResultMap<TValueIn, TValueOut>(this IResultValue<TValueIn> @this,
                                                                             Func<TValueIn, TValueOut> func)
        {
            if (func == null) throw new ArgumentNullException(nameof(func));
            if (@this == null) return null;

            if (@this.HasErrors) return new ResultValue<TValueOut>(@this.Errors);

            return
            new ResultValue<TValueOut>(func(@this.Value), @this.Errors);
        }

        public static ResultValue<T> ToResultValue<T>(this T @this)
        {
            if (@this == null) return null;

            return new ResultValue<T>(@this);
        }
    }
}
