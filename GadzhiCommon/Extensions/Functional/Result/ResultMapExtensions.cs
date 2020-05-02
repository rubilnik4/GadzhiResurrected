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
        /// Выполнить действие, вернуть результирующий ответ
        /// </summary>      
        public static IResultValue<TValue> ResultVoid<TValue>(this IResultValue<TValue> @this, Action<TValue> action)
        {
            if (@this == null) throw new ArgumentNullException(nameof(@this));

            action?.Invoke(@this.Value);
            return @this;
        }

        /// <summary>
        /// Выполнить действие при положительном значении, вернуть результирующий ответ
        /// </summary>      
        public static IResultValue<TValue> ResultVoidOk<TValue>(this IResultValue<TValue> @this, Action<TValue> action)
        {
            if (@this == null) throw new ArgumentNullException(nameof(@this));

            if (@this?.OkStatus == true) action?.Invoke(@this.Value);
            return @this;
        }
    }
}
