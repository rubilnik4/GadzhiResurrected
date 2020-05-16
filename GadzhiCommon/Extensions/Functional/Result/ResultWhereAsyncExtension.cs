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
        /// Выполнение положительного условия или возвращение предыдущей ошибки в результирующем ответе
        /// </summary>   
        public static async Task<IResultValue<TValueOut>> ResultValueOkAsync<TValueIn, TValueOut>(this Task<IResultValue<TValueIn>> @this, 
                                                                                                  Func<TValueIn, TValueOut> okFunc)
        {
            if (okFunc == null) throw new ArgumentNullException(nameof(okFunc));
            if (@this == null) throw new ArgumentNullException(nameof(@this));

            var awaitedThis = await @this;
            if (awaitedThis.HasErrors) return new ResultValue<TValueOut>(awaitedThis.Errors);

            return okFunc.Invoke(awaitedThis.Value).
                          Map(okResult => new ResultValue<TValueOut>(okResult, awaitedThis.Errors));
        }

        /// <summary>
        /// Выполнение положительного условия или возвращение предыдущей ошибки в результирующем ответе
        /// </summary>   
        public static async Task<IResultValue<TValueOut>> ResultValueOkAsync<TValueIn, TValueOut>(this IResultValue<TValueIn> @this,
                                                                                                  Func<TValueIn, Task<TValueOut>> okFunc)
        {
            if (okFunc == null) throw new ArgumentNullException(nameof(okFunc));
            if (@this == null) throw new ArgumentNullException(nameof(@this));

            if (@this.HasErrors) return new ResultValue<TValueOut>(@this.Errors);

            return await okFunc.Invoke(@this.Value).
                                MapAsync(okResult => new ResultValue<TValueOut>(okResult, @this.Errors));
        }

        /// <summary>
        /// Выполнение положительного условия результирующего ответа со связыванием или возвращение предыдущей ошибки в результирующем ответе асинхронно
        /// </summary>   
        public static async Task<IResultValue<TValueOut>> ResultValueOkBindAsync<TValueIn, TValueOut>(this IResultValue<TValueIn> @this,
                                                                                                      Func<TValueIn, Task<IResultValue<TValueOut>>> okFunc)
        {
            if (okFunc == null) throw new ArgumentNullException(nameof(okFunc));

            if (@this == null) throw new ArgumentNullException(nameof(@this));

            return @this.HasErrors
                ? new ResultValue<TValueOut>(@this.Errors)
                : await okFunc.Invoke(@this.Value);
        }
    }
}