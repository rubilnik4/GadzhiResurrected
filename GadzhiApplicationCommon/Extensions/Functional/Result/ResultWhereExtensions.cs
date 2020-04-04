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
        public static IResultApplicationValue<TValueOut> ResultContinue<TValueIn, TValueOut>(this IResultApplicationValue<TValueIn> @this,
                                                                                            Func<TValueIn, bool> predicate,
                                                                                            Func<TValueIn, TValueOut> okFunc,
                                                                                            Func<TValueIn, IEnumerable<IErrorApplication>> badFunc)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            if (okFunc == null) throw new ArgumentNullException(nameof(okFunc));
            if (badFunc == null) throw new ArgumentNullException(nameof(badFunc));

            if (@this == null) return null;
            if (@this.HasErrors) return new ResultApplicationValue<TValueOut>(@this.Errors);

            return predicate(@this.Value) ?
                   okFunc.Invoke(@this.Value).
                          Map(okResult => new ResultApplicationValue<TValueOut>(okResult, @this.Errors)) :
                   badFunc.Invoke(@this.Value).
                          Map(badResult => new ResultApplicationValue<TValueOut>(@this.Errors.Union(badResult)));
        }

        /// <summary>
        /// Выполнение положительного условия или возвращение предыдущей ошибки в результирующем ответе
        /// </summary>   
        public static IResultApplicationValue<TValueOut> ResultValueOk<TValueIn, TValueOut>(this IResultApplicationValue<TValueIn> @this,
                                                                                            Func<TValueIn, TValueOut> okFunc)

        {
            if (okFunc == null) throw new ArgumentNullException(nameof(okFunc));

            if (@this == null) return null;
            if (@this.HasErrors) return new ResultApplicationValue<TValueOut>(@this.Errors);

            return okFunc.Invoke(@this.Value).
                   Map(okResult => new ResultApplicationValue<TValueOut>(okResult, @this.Errors));
        }

        /// <summary>
        /// Выполнение положительного условия результирующего ответа или возвращение предыдущей ошибки в результирующем ответе
        /// </summary>   
        public static IResultApplicationValue<TValueOut> ResultValueOkBind<TValueIn, TValueOut>(this IResultApplicationValue<TValueIn> @this,
                                                                                                Func<TValueIn, IResultApplicationValue<TValueOut>> okFunc)
        {
            if (okFunc == null) throw new ArgumentNullException(nameof(okFunc));

            if (@this == null) return null;
            if (@this.HasErrors) return new ResultApplicationValue<TValueOut>(@this.Errors);

            return okFunc.Invoke(@this.Value).
                   WhereContinue(result => result.OkStatus,
                        okFunc: result => new ResultApplicationValue<TValueOut>(result.Value),
                        badFunc: result => new ResultApplicationValue<TValueOut>(result.Errors));
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
