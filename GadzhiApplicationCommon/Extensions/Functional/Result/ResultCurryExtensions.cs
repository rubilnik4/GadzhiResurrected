using System;
using System.Linq;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Interfaces.Errors;

namespace GadzhiApplicationCommon.Extensions.Functional.Result
{
    /// <summary>
    /// Преобразование внутреннего типа результирующего ответа для функций высшего порядка
    /// </summary>
    public static class ResultCurryExtensions
    {
        /// <summary>
        /// Преобразование результирующего ответа с функцией высшего порядка для одного аргумента
        /// </summary>
        public static IResultAppValue<Func<TOut>> ResultCurryOkBind<TIn1, TOut>(this IResultAppValue<Func<TIn1, TOut>> @this,
                                                                                IResultAppValue<TIn1> arg1)
        {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (arg1 == null) throw new ArgumentNullException(nameof(arg1));

            if (@this.HasErrors) return new ResultAppValue<Func<TOut>>(@this.Errors);

            return arg1.OkStatus
               ? new ResultAppValue<Func<TOut>>(@this.Value.Curry(arg1.Value))
               : new ResultAppValue<Func<TOut>>(@this.Errors.Concat(arg1.Errors));
        }

        /// <summary>
        /// Преобразование результирующего ответа с функцией высшего порядка для двух аргументов
        /// </summary>
        public static IResultAppValue<Func<TIn2, TOut>> ResultCurryOkBind<TIn1, TIn2, TOut>(this IResultAppValue<Func<TIn1, TIn2, TOut>> @this,
                                                                                            IResultAppValue<TIn1> arg1)
        {
            if (@this == null) throw new ArgumentNullException(nameof(@this));
            if (arg1 == null) throw new ArgumentNullException(nameof(arg1));

            if (@this.HasErrors) return new ResultAppValue<Func<TIn2, TOut>>(@this.Errors);

            return arg1.OkStatus
               ? new ResultAppValue<Func<TIn2, TOut>>(@this.Value.Curry(arg1.Value))
               : new ResultAppValue<Func<TIn2, TOut>>(@this.Errors.Concat(arg1.Errors));
        }
    }
}