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
        public static IResultConvertingValue<TValueOut> ResultMap<TValueIn, TValueOut>(this IResultConvertingValue<TValueIn> @this,
                                                                                       Func<TValueIn, TValueOut> func)
        {
            if (func == null) throw new ArgumentNullException(nameof(func));
            if (@this == null) return null;

            if (@this.HasErrors) return new ResultConvertingValue<TValueOut>(@this.Errors);

            return
            new ResultConvertingValue<TValueOut>(func(@this.Value), @this.Errors);
        }
    }
}
