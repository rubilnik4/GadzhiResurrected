using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiCommon.Extentions.Functional
{
    /// <summary>
    /// Методы расширения для проверки условий
    /// </summary>
    public static class WhereExtensions
    {
        /// <summary>
        /// Условие возвращающее объект или null 
        /// </summary>      
        public static T WhereNull<T>(this T @this, Func<T, bool> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            return predicate(@this) ?
                   @this :
                   default;
        }

        /// <summary>
        /// Условие продолжающее действие
        /// </summary>      
        public static TResult WhereContinue<TSource, TResult>(this TSource @this, Func<TSource, bool> predicate,
                                                              Func<TSource, TResult> okFunc, Func<TSource, TResult> badFunc)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            if (okFunc == null) throw new ArgumentNullException(nameof(okFunc));
            if (badFunc == null) throw new ArgumentNullException(nameof(badFunc));

            return predicate(@this) ?
                   okFunc.Invoke(@this) :
                   badFunc.Invoke(@this);
        }

        /// <summary>
        /// Обработка позитивного условия
        /// </summary>      
        public static TSource WhereOk<TSource>(this TSource @this, Func<TSource, bool> predicate, Func<TSource, TSource> okFunc) =>
               @this.WhereContinue(predicate, okFunc, _ => @this);

        /// <summary>
        /// Обработка негативного условия
        /// </summary>      
        public static TSource WhereBad<TSource>(this TSource @this, Func<TSource, bool> predicate, Func<TSource, TSource> badFunc) =>
               @this.WhereContinue(predicate, _ => @this, badFunc);
    }
}
