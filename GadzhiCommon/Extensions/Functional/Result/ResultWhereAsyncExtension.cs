using System;
using System.Threading.Tasks;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiCommon.Models.Interfaces.Errors;

namespace GadzhiCommon.Extensions.Functional.Result
{
    /// <summary>
    /// Обработка условий для асинхронного результирующего ответа
    /// </summary>
    public static class ResultWhereAsyncExtension
    {
        /// <summary>
        /// Выполнение положительного условия результирующего ответа со связыванием или возвращение предыдущей ошибки в результирующем ответе асинхронно
        /// </summary>   
        public static async Task<IResultValue<TValueOut>> ResultValueOkBindAsync<TValueIn, TValueOut>(this IResultValue<TValueIn> @this,
                                                                                     Func<TValueIn, Task<IResultValue<TValueOut>>> okFunc)
        {
            if (okFunc == null) throw new ArgumentNullException(nameof(okFunc));

            if (@this == null) return null;

            return @this.HasErrors
                ? new ResultValue<TValueOut>(@this.Errors)
                : await okFunc.Invoke(@this.Value);
        }
    }
}