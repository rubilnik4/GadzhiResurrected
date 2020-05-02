using System;
using System.Threading.Tasks;

namespace GadzhiCommon.Extensions.Functional
{
    /// <summary>
    /// Методы расширения для проверки условий с асинхронным выполнением
    /// </summary>
    public static class WhereAsyncExtensions
    {
        /// <summary>
        /// Условие продолжающее действие с асинхронным выполнением
        /// </summary>      
        public static async Task<TResult> WhereContinueAsync<TSource, TResult>(this TSource @this, Func<TSource, bool> predicate,
                                                              Func<TSource, Task<TResult>> okFunc, Func<TSource, Task<TResult>> badFunc)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            if (okFunc == null) throw new ArgumentNullException(nameof(okFunc));
            if (badFunc == null) throw new ArgumentNullException(nameof(badFunc));

            return predicate(@this) ?
                   await okFunc.Invoke(@this) :
                   await badFunc.Invoke(@this);
        }

        /// <summary>
        /// Обработка позитивного условия
        /// </summary>      
        public static async Task<TSource> WhereOkAsync<TSource>(this TSource @this, Func<TSource, bool> predicate,
                                                                Func<TSource, Task<TSource>> okFunc) =>
               await @this.WhereContinueAsync(predicate, okFunc, _ => new Task<TSource>(() => @this));
    }
}
