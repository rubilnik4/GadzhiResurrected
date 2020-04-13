using GadzhiCommon.Models.Implementations.Errors;
using GadzhiCommon.Models.Interfaces.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GadzhiConverting.Infrastructure.Implementations.ExecuteBindHandler;

namespace GadzhiCommon.Extentions.Functional.Result
{
    /// <summary>
    /// Обработка условий для результируещего ответа
    /// </summary>
    public static class ResultWhereExtensions
    {

        /// <summary>
        /// Выполнение условия или возвращение предыдущей ошибки в результирующем ответе
        /// </summary>      
        public static IResultValue<TValueOut> ResultContinue<TValueIn, TValueOut>(this IResultValue<TValueIn> @this,
                                                                                            Func<TValueIn, bool> predicate,
                                                                                            Func<TValueIn, TValueOut> okFunc,
                                                                                            Func<TValueIn, IEnumerable<IErrorCommon>> badFunc)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            if (okFunc == null) throw new ArgumentNullException(nameof(okFunc));
            if (badFunc == null) throw new ArgumentNullException(nameof(badFunc));

            if (@this == null) return null;
            if (@this.HasErrors) return new ResultValue<TValueOut>(@this.Errors);

            return predicate(@this.Value) ?
                   okFunc.Invoke(@this.Value).
                          Map(okResult => new ResultValue<TValueOut>(okResult, @this.Errors)) :
                   badFunc.Invoke(@this.Value).
                          Map(badResult => new ResultValue<TValueOut>(@this.Errors.Union(badResult)));
        }

        /// <summary>
        /// Выполнение положительного условия результирующего ответа или возвращение предыдущей ошибки в результирующем ответе
        /// </summary>   
        //public static IResult ResultOkBind(this IResult @this, Func<IResult> okFunc)
        //{
        //    if (okFunc == null) throw new ArgumentNullException(nameof(okFunc));

        //    if (@this == null) return null;
        //    if (@this.HasErrors) return new Models.Implementations.Errors.Result(@this.Errors);

        //    return okFunc.Invoke().
        //           WhereContinue(result => result.OkStatus,
        //                okFunc: result => new Models.Implementations.Errors.Result(),
        //                badFunc: result => new Models.Implementations.Errors.Result(result.Errors));
        //}

        /// <summary>
        /// Выполнение положительного условия или возвращение предыдущей ошибки в результирующем ответе
        /// </summary>   
        public static IResultValue<TValueOut> ResultValueOk<TValueIn, TValueOut>(this IResultValue<TValueIn> @this,
                                                                                      Func<TValueIn, TValueOut> okFunc)
                                                                                          
        {           
            if (okFunc == null) throw new ArgumentNullException(nameof(okFunc));          

            if (@this == null) return null;
            if (@this.HasErrors) return new ResultValue<TValueOut>(@this.Errors);

            return okFunc.Invoke(@this.Value).
                   Map(okResult => new ResultValue<TValueOut>(okResult, @this.Errors));
        }

        /// <summary>
        /// Выполнение положительного условия результирующего ответа со связыванием или возвращение предыдущей ошибки в результирующем ответе
        /// </summary>   
        public static IResultValue<TValueOut> ResultValueOkBind<TValueIn, TValueOut>(this IResultValue<TValueIn> @this,
                                                                                     Func<TValueIn, IResultValue<TValueOut>> okFunc)
        {
            if (okFunc == null) throw new ArgumentNullException(nameof(okFunc));

            if (@this == null) return null;
            if (@this.HasErrors) return new ResultValue<TValueOut>(@this.Errors);

            return okFunc.Invoke(@this.Value).
                   WhereContinue(result => result.OkStatus,
                        okFunc: result => new ResultValue<TValueOut>(result.Value),
                        badFunc: result => new ResultValue<TValueOut>(result.Errors));
        }

        /// <summary>
        /// Выполнение положительного условия результирующего ответа или возвращение предыдущей ошибки в результирующем ответе с обработкой ошибок
        /// </summary>   
        public static IResultValue<TValueOut> ResultValueOkTry<TValueIn, TValueOut>(this IResultValue<TValueIn> @this,
                                                                                              Func<TValueIn, IResultValue<TValueOut>> okFunc,
                                                                                              IErrorCommon errorMessage = null)
        {
            if (okFunc == null) throw new ArgumentNullException(nameof(okFunc));

            if (@this == null) return null;
            if (@this.HasErrors) return new ResultValue<TValueOut>(@this.Errors);

            return ExecuteBindFileDataErrors<TValueOut, IResultValue<TValueOut>>(() => okFunc(@this.Value), errorMessage);
        }      
    }
}
