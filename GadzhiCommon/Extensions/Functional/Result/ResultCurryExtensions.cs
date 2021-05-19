using System;
using System.Linq;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiCommon.Models.Interfaces.Errors;

namespace GadzhiCommon.Extensions.Functional.Result
{
    /// <summary>
    /// Преобразование внутреннего типа результирующего ответа для функций высшего порядка
    /// </summary>
    public static class ResultCurryExtensions
    {
        /// <summary>
        /// Преобразование результирующего ответа с функцией высшего порядка для одного аргумента
        /// </summary>
        public static IResultValue<Func<TOut>> ResultCurryOkBind<TIn1, TOut>(this IResultValue<Func<TIn1, TOut>> @this,
                                                                             IResultValue<TIn1> arg1)
        {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (arg1 == null) throw new ArgumentNullException(nameof(arg1));

            if (@this.HasErrors) return new ResultValue<Func<TOut>>(@this.Errors);

            return arg1.OkStatus
               ? new ResultValue<Func<TOut>>(@this.Value.Curry(arg1.Value))
               : new ResultValue<Func<TOut>>(@this.Errors.Concat(arg1.Errors));
        }

        /// <summary>
        /// Преобразование результирующего ответа с функцией высшего порядка для двух аргументов
        /// </summary>
        public static IResultValue<Func<TIn2, TOut>> ResultCurryOkBind<TIn1, TIn2, TOut>(this IResultValue<Func<TIn1, TIn2, TOut>> @this,
                                                                                            IResultValue<TIn1> arg1)
        {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (arg1 == null) throw new ArgumentNullException(nameof(arg1));

            if (@this.HasErrors) return new ResultValue<Func<TIn2, TOut>>(@this.Errors);

            return arg1.OkStatus
               ? new ResultValue<Func<TIn2, TOut>>(@this.Value.Curry(arg1.Value))
               : new ResultValue<Func<TIn2, TOut>>(@this.Errors.Concat(arg1.Errors));
        }

        /// <summary>
        /// Преобразование результирующего ответа с функцией высшего порядка для трех аргументов
        /// </summary>
        public static IResultValue<Func<TIn2, TIn3, TOut>> ResultCurryOkBind<TIn1, TIn2, TIn3, TOut>(this IResultValue<Func<TIn1, TIn2, TIn3, TOut>> @this,
                                                                                            IResultValue<TIn1> arg1)
        {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (arg1 == null) throw new ArgumentNullException(nameof(arg1));

            if (@this.HasErrors) return new ResultValue<Func<TIn2, TIn3, TOut>>(@this.Errors);

            return arg1.OkStatus
               ? new ResultValue<Func<TIn2, TIn3, TOut>>(@this.Value.Curry(arg1.Value))
               : new ResultValue<Func<TIn2, TIn3, TOut>>(@this.Errors.Concat(arg1.Errors));
        }

        /// <summary>
        /// Преобразование результирующего ответа с функцией высшего порядка для трех аргументов
        /// </summary>
        public static IResultValue<Func<TIn2, TIn3, TIn4, TOut>> ResultCurryOkBind<TIn1, TIn2, TIn3, TIn4, TOut>(this IResultValue<Func<TIn1, TIn2, TIn3, TIn4, TOut>> @this,
                                                                                                                 IResultValue<TIn1> arg1)
        {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (arg1 == null) throw new ArgumentNullException(nameof(arg1));

            if (@this.HasErrors) return new ResultValue<Func<TIn2, TIn3, TIn4, TOut>>(@this.Errors);

            return arg1.OkStatus
               ? new ResultValue<Func<TIn2, TIn3, TIn4, TOut>>(@this.Value.Curry(arg1.Value))
               : new ResultValue<Func<TIn2, TIn3, TIn4, TOut>>(@this.Errors.Concat(arg1.Errors));
        }
    }
}