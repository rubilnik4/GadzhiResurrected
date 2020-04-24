using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiApplicationCommon.Extensions.Functional.Result
{
    /// <summary>
    /// Обработка условий для результируещего ответа
    /// </summary>
    public static class ResultWhereExtensions
    {

        /// <summary>
        /// Выполнение условия или возвращение предыдущей ошибки в результирующем ответе
        /// </summary>      
        public static IResultAppValue<TValueOut> ResultValueContinue<TValueIn, TValueOut>(this IResultAppValue<TValueIn> @this,
                                                                                            Func<TValueIn, bool> predicate,
                                                                                            Func<TValueIn, TValueOut> okFunc,
                                                                                            Func<TValueIn, IEnumerable<IErrorApplication>> badFunc)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            if (okFunc == null) throw new ArgumentNullException(nameof(okFunc));
            if (badFunc == null) throw new ArgumentNullException(nameof(badFunc));

            if (@this == null) return null;
            if (@this.HasErrors) return new ResultAppValue<TValueOut>(@this.Errors);

            return predicate(@this.Value) ?
                   okFunc.Invoke(@this.Value).
                          Map(okResult => new ResultAppValue<TValueOut>(okResult, @this.Errors)) :
                   badFunc.Invoke(@this.Value).
                          Map(badResult => new ResultAppValue<TValueOut>(@this.Errors.Union(badResult)));
        }

        /// <summary>
        /// Выполнение положительного иди негативного условия в результирующем ответе
        /// </summary>      
        public static IResultAppValue<TValueOut> ResultOkBad<TValueIn, TValueOut>(this IResultAppValue<TValueIn> @this,
                                                                               Func<TValueIn, TValueOut> okFunc,
                                                                               Func<TValueIn, TValueOut> badFunc)
        {
            if (okFunc == null) throw new ArgumentNullException(nameof(okFunc));
            if (badFunc == null) throw new ArgumentNullException(nameof(badFunc));

            if (@this == null) return null;

            return @this.OkStatus ?
                   okFunc.Invoke(@this.Value).
                          Map(okResult => new ResultAppValue<TValueOut>(okResult, @this.Errors)) :
                   badFunc.Invoke(@this.Value).
                           Map(badResult => new ResultAppValue<TValueOut>(badResult, @this.Errors));
        }


        /// <summary>
        /// Выполнение положительного условия или возвращение предыдущей ошибки в результирующем ответе
        /// </summary>   
        public static IResultAppValue<TValueOut> ResultValueOk<TValueIn, TValueOut>(this IResultAppValue<TValueIn> @this,
                                                                                            Func<TValueIn, TValueOut> okFunc)

        {
            if (okFunc == null) throw new ArgumentNullException(nameof(okFunc));

            if (@this == null) return null;
            if (@this.HasErrors) return new ResultAppValue<TValueOut>(@this.Errors);

            return okFunc.Invoke(@this.Value).
                   Map(okResult => new ResultAppValue<TValueOut>(okResult, @this.Errors));
        }

        /// <summary>
        /// Выполнение негативного условия или возвращение положительного условия в результирующем ответе
        /// </summary>   
        public static IResultAppValue<TValue> ResultValueBad<TValue>(this IResultAppValue<TValue> @this, Func<TValue, TValue> badFunc)
        {
            if (badFunc == null) throw new ArgumentNullException(nameof(badFunc));

            if (@this == null) return null;
            if (@this.OkStatus) return @this;

            return badFunc.Invoke(@this.Value).
                   Map(badResult => new ResultAppValue<TValue>(badResult));
        }

        /// <summary>
        /// Выполнение положительного условия результирующего ответа или возвращение предыдущей ошибки в результирующем ответе
        /// </summary>   
        public static IResultAppValue<TValueOut> ResultValueOkBind<TValueIn, TValueOut>(this IResultAppValue<TValueIn> @this,
                                                                                                Func<TValueIn, IResultAppValue<TValueOut>> okFunc)
        {
            if (okFunc == null) throw new ArgumentNullException(nameof(okFunc));

            if (@this == null) return null;
            if (@this.HasErrors) return new ResultAppValue<TValueOut>(@this.Errors);

            return okFunc.Invoke(@this.Value).
                   WhereContinue(result => result.OkStatus,
                        okFunc: result => new ResultAppValue<TValueOut>(result.Value),
                        badFunc: result => new ResultAppValue<TValueOut>(result.Value, result.Errors));
        }

        /// <summary>
        /// Выполнение положительного условия результирующего ответа или возвращение предыдущей ошибки в результирующем ответе
        /// </summary>   
        public static IResultApplication ResultOkBind(this IResultApplication @this, Func<IResultApplication> okFunc)
        {
            if (okFunc == null) throw new ArgumentNullException(nameof(okFunc));

            if (@this == null) return null;
            if (@this.HasErrors) return new ResultApplication(@this.Errors);

            return okFunc.Invoke().
                   WhereContinue(result => result.OkStatus,
                        okFunc: result => new ResultApplication(),
                        badFunc: result => new ResultApplication(result.Errors));
        }
    }
}
