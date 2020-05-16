using System;
using System.Collections.Generic;
using System.Linq;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiCommon.Models.Interfaces.Errors;
using static GadzhiCommon.Extensions.Functional.ExecuteBindHandler;

namespace GadzhiCommon.Extensions.Functional.Result
{
    /// <summary>
    /// Обработка условий для результирующего ответа
    /// </summary>
    public static class ResultWhereExtensions
    {

        /// <summary>
        /// Выполнение условия или возвращение предыдущей ошибки в результирующем ответе
        /// </summary>      
        public static IResultValue<TValueOut> ResultValueContinue<TValueIn, TValueOut>(this IResultValue<TValueIn> @this,
                                                                                  Func<TValueIn, bool> predicate,
                                                                                  Func<TValueIn, TValueOut> okFunc,
                                                                                  Func<TValueIn, IEnumerable<IErrorCommon>> badFunc)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            if (okFunc == null) throw new ArgumentNullException(nameof(okFunc));
            if (badFunc == null) throw new ArgumentNullException(nameof(badFunc));
            if (@this == null) throw new ArgumentNullException(nameof(@this));

            if (@this.HasErrors) return new ResultValue<TValueOut>(@this.Errors);

            return predicate(@this.Value) ?
                   okFunc.Invoke(@this.Value).
                          Map(okResult => new ResultValue<TValueOut>(okResult, @this.Errors)) :
                   badFunc.Invoke(@this.Value).
                           Map(badResult => new ResultValue<TValueOut>(@this.Errors.Union(badResult)));
        }

        /// <summary>
        /// Выполнение положительного иди негативного условия в результирующем ответе
        /// </summary>      
        public static IResultValue<TValueOut> ResultOkBad<TValueIn, TValueOut>(this IResultValue<TValueIn> @this,
                                                                               Func<TValueIn, TValueOut> okFunc,
                                                                               Func<TValueIn, TValueOut> badFunc)
        {
            if (okFunc == null) throw new ArgumentNullException(nameof(okFunc));
            if (badFunc == null) throw new ArgumentNullException(nameof(badFunc));
            if (@this == null) throw new ArgumentNullException(nameof(@this));

            return @this.OkStatus ?
                   okFunc.Invoke(@this.Value).
                          Map(okResult => new ResultValue<TValueOut>(okResult, @this.Errors)) :
                   badFunc.Invoke(@this.Value).
                           Map(badResult => new ResultValue<TValueOut>(badResult, @this.Errors));
        }

        /// <summary>
        /// Выполнение положительного условия или возвращение предыдущей ошибки в результирующем ответе
        /// </summary>   
        public static IResultValue<TValueOut> ResultValueOk<TValueIn, TValueOut>(this IResultValue<TValueIn> @this, Func<TValueIn, TValueOut> okFunc)
        {
            if (okFunc == null) throw new ArgumentNullException(nameof(okFunc));
            if (@this == null) throw new ArgumentNullException(nameof(@this));

            if (@this.HasErrors) return new ResultValue<TValueOut>(@this.Errors);

            return okFunc.Invoke(@this.Value).
                   Map(okResult => new ResultValue<TValueOut>(okResult, @this.Errors));
        }

        /// <summary>
        /// Выполнение положительного условия или возвращение предыдущей ошибки в результирующем ответе в обертке
        /// </summary>   
        public static IResultValue<TValueOut> ResultValueOkRaw<TValueIn, TValueOut>(this IResultValue<TValueIn> @this, Func<IResultValue<TValueIn>, IResultValue<TValueOut>> okFunc)
        {
            if (okFunc == null) throw new ArgumentNullException(nameof(okFunc));
            if (@this == null) throw new ArgumentNullException(nameof(@this));

            return @this.HasErrors 
                ? new ResultValue<TValueOut>(@this.Errors) 
                : okFunc.Invoke(@this);
        }

        /// <summary>
        /// Выполнение положительного условия или возвращение предыдущей ошибки в результирующем ответе в обертке
        /// </summary>   
        public static IResultCollection<TValueOut> ResultValueOkRawCollection<TValueIn, TValueOut>(this IResultCollection<TValueIn> @this,
                                                                                                   Func<IResultCollection<TValueIn>,
                                                                                                   IResultCollection<TValueOut>> okFunc)
        {
            if (okFunc == null) throw new ArgumentNullException(nameof(okFunc));
            if (@this == null) throw new ArgumentNullException(nameof(@this));

            return @this.HasErrors 
                ? new ResultCollection<TValueOut>(@this.Errors) 
                : okFunc.Invoke(@this);
        }

        /// <summary>
        /// Выполнение положительного условия или возвращение предыдущей ошибки в результирующем ответе в обертке при равенстве параметров
        /// </summary>   
        public static IResultCollection<TValue> ResultValueEqualOkRawCollection<TValue>(this IResultCollection<TValue> @this,
                                                                                        Func<IResultCollection<TValue>,
                                                                                        IResultCollection<TValue>> okFunc)
        {
            if (okFunc == null) throw new ArgumentNullException(nameof(okFunc));
            if (@this == null) throw new ArgumentNullException(nameof(@this));

            return @this.HasErrors 
                ? new ResultCollection<TValue>(@this.Value, @this.Errors)
                : okFunc.Invoke(@this);
        }

        /// <summary>
        /// Выполнение негативного условия или возвращение положительного условия в результирующем ответе
        /// </summary>   
        public static IResultValue<TValue> ResultValueBad<TValue>(this IResultValue<TValue> @this, Func<TValue, TValue> badFunc)
        {
            if (badFunc == null) throw new ArgumentNullException(nameof(badFunc));
            if (@this == null) throw new ArgumentNullException(nameof(@this));

            if (@this.OkStatus) return @this;

            return badFunc.Invoke(@this.Value).
                   Map(badResult => new ResultValue<TValue>(badResult));
        }

        /// <summary>
        /// Выполнение положительного условия результирующего ответа со связыванием или возвращение предыдущей ошибки в результирующем ответе
        /// </summary>   
        public static IResultValue<TValueOut> ResultValueOkBind<TValueIn, TValueOut>(this IResultValue<TValueIn> @this,
                                                                                     Func<TValueIn, IResultValue<TValueOut>> okFunc)
        {
            if (okFunc == null) throw new ArgumentNullException(nameof(okFunc));
            if (@this == null) throw new ArgumentNullException(nameof(@this));

            return @this.HasErrors 
                ? new ResultValue<TValueOut>(@this.Errors) 
                : okFunc.Invoke(@this.Value);
        }

        /// <summary>
        /// Выполнение негативного условия результирующего ответа или возвращение положительного в результирующем ответе
        /// </summary>   
        public static IResultValue<TValue> ResultValueBadBind<TValue>(this IResultValue<TValue> @this,
                                                                      Func<TValue, IResultValue<TValue>> badFunc)
        {
            if (badFunc == null) throw new ArgumentNullException(nameof(badFunc));
            if (@this == null) throw new ArgumentNullException(nameof(@this));

            return @this.OkStatus 
                ? @this 
                : badFunc.Invoke(@this.Value);
        }

        /// <summary>
        /// Выполнение положительного условия результирующего ответа или возвращение предыдущей ошибки в результирующем ответе с обработкой ошибок
        /// </summary>   
        public static IResultValue<TValueOut> ResultValueOkTry<TValueIn, TValueOut>(this IResultValue<TValueIn> @this,
                                                                                    Func<TValueIn, IResultValue<TValueOut>> okFunc,
                                                                                    IErrorCommon errorMessage = null)
        {
            if (okFunc == null) throw new ArgumentNullException(nameof(okFunc));
            if (@this == null) throw new ArgumentNullException(nameof(@this));

            return @this.HasErrors 
                ? new ResultValue<TValueOut>(@this.Errors)
                : ExecuteBindResultValue(() => okFunc(@this.Value), errorMessage);
        }
    }
}
