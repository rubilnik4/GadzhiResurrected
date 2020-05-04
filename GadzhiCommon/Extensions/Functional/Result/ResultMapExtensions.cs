using System;
using GadzhiCommon.Models.Interfaces.Errors;

namespace GadzhiCommon.Extensions.Functional.Result
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

            if (@this.OkStatus) action?.Invoke(@this.Value);
            return @this;
        }
    }
}
