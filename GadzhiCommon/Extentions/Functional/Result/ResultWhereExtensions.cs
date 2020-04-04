using GadzhiCommon.Models.Implementations.Errors;
using GadzhiCommon.Models.Interfaces.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public static IResultConvertingValue<TValueOut> ResultContinue<TValueIn, TValueOut>(this IResultConvertingValue<TValueIn> @this,
                                                                                            Func<TValueIn, bool> predicate,
                                                                                            Func<TValueIn, TValueOut> okFunc,
                                                                                            Func<TValueIn, IEnumerable<IErrorConverting>> badFunc)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            if (okFunc == null) throw new ArgumentNullException(nameof(okFunc));
            if (badFunc == null) throw new ArgumentNullException(nameof(badFunc));

            if (@this == null) return null;
            if (@this.HasErrors) return new ResultConvertingValue<TValueOut>(@this.Errors);

            return predicate(@this.Value) ?
                   okFunc.Invoke(@this.Value).
                          Map(okResult => new ResultConvertingValue<TValueOut>(okResult, @this.Errors)) :
                   badFunc.Invoke(@this.Value).
                          Map(badResult => new ResultConvertingValue<TValueOut>(@this.Errors.Union(badResult)));
        }

        /// <summary>
        /// Выполнение положительного условия или возвращение предыдущей ошибки в результирующем ответе
        /// </summary>   
        public static IResultConvertingValue<TValueOut> ResultValueOk<TValueIn, TValueOut>(this IResultConvertingValue<TValueIn> @this,
                                                                                      Func<TValueIn, TValueOut> okFunc)
                                                                                          
        {           
            if (okFunc == null) throw new ArgumentNullException(nameof(okFunc));          

            if (@this == null) return null;
            if (@this.HasErrors) return new ResultConvertingValue<TValueOut>(@this.Errors);

            return okFunc.Invoke(@this.Value).
                   Map(okResult => new ResultConvertingValue<TValueOut>(okResult, @this.Errors));
        }

        /// <summary>
        /// Выполнение положительного условия результирующего ответа или возвращение предыдущей ошибки в результирующем ответе
        /// </summary>   
        public static IResultConvertingValue<TValueOut> ResultValueOkBind<TValueIn, TValueOut>(this IResultConvertingValue<TValueIn> @this,
                                                                                          Func<TValueIn, IResultConvertingValue<TValueOut>> okFunc)
        {
            if (okFunc == null) throw new ArgumentNullException(nameof(okFunc));

            if (@this == null) return null;
            if (@this.HasErrors) return new ResultConvertingValue<TValueOut>(@this.Errors);

            return okFunc.Invoke(@this.Value).
                   WhereContinue(result => result.OkStatus,
                        okFunc: result => new ResultConvertingValue<TValueOut>(result.Value),
                        badFunc: result => new ResultConvertingValue<TValueOut>(result.Errors));
        }

        /// <summary>
        /// Выполнение положительного условия результирующего ответа или возвращение предыдущей ошибки в результирующем ответе
        /// </summary>   
        public static IResultConverting ResultOkBind(this IResultConverting @this, Func<IResultConverting> okFunc)
        {
            if (okFunc == null) throw new ArgumentNullException(nameof(okFunc));

            if (@this == null) return null;
            if (@this.HasErrors) return new ResultConverting(@this.Errors);

            return okFunc.Invoke().
                   WhereContinue(result => result.OkStatus,
                        okFunc: result => new ResultConverting(),
                        badFunc: result => new ResultConverting(result.Errors));
        }
    }
}
