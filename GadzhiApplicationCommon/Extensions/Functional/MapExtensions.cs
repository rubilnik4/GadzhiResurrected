using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiApplicationCommon.Extensions.Functional
{
    /// <summary>
    /// Методы расширения для преобразования типов
    /// </summary>
    public static class MapExtensions
    {
        /// <summary>
        /// Преобразование типов с помощью функции
        /// </summary>       
        public static TResult Map<TSource, TResult>(this TSource @this, Func<TSource, TResult> func) =>
            func != null ?
            func(@this) :
            throw new ArgumentNullException(nameof(func));


        /// <summary>
        /// Условие возвращающее объект или null 
        /// </summary>      
        public static T WhereNull<T>(this T @this, Func<T, bool> predicate) =>
            predicate != null ?
                predicate(@this) ?
                @this :
                default
            :
            throw new ArgumentNullException(nameof(predicate));

        /// <summary>
        /// Условие продолжающее действие
        /// </summary>      
        public static TResult WhereContinue<TSource, TResult>(this TSource @this, Func<TSource, bool> predicate, 
                                                              Func<TSource, TResult> okFunc, Func<TSource, TResult> badFunc)
        {
            if (okFunc == null) throw new ArgumentNullException(nameof(okFunc));
            if (badFunc == null) throw new ArgumentNullException(nameof(badFunc));
            return predicate != null ?
                    predicate(@this) ?
                    okFunc.Invoke(@this) :
                    badFunc.Invoke(@this)
                   :
                   throw new ArgumentNullException(nameof(predicate));
        }

        /// <summary>
        /// Обработка позитивного условия
        /// </summary>      
        public static TSource WhereOK<TSource>(this TSource @this, Func<TSource, bool> predicate, Func<TSource, TSource> okFunc) =>
               @this.WhereContinue(predicate, okFunc, (_) => @this);

        /// <summary>
        /// Обработка негативного условия
        /// </summary>      
        public static TSource WhereBad<TSource>(this TSource @this, Func<TSource, bool> predicate, Func<TSource, TSource> badFunc) =>
               @this.WhereContinue(predicate, (_) => @this, badFunc);
    }
}
