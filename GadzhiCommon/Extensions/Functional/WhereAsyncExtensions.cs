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
        public static async Task<TResult> WhereContinueAsyncBind<TSource, TResult>(this TSource @this, Func<TSource, bool> predicate,
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
        /// Условие продолжающее действие с асинхронным выполнением
        /// </summary>      
        public static async Task<TResult> WhereContinueAsync<TSource, TResult>(this Task<TSource> @this, Func<TSource, bool> predicate,
                                                                               Func<TSource, TResult> okFunc, Func<TSource, TResult> badFunc)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            if (okFunc == null) throw new ArgumentNullException(nameof(okFunc));
            if (badFunc == null) throw new ArgumentNullException(nameof(badFunc));

            var awaitedAsync = await @this;

            return predicate(awaitedAsync) ?
                okFunc.Invoke(awaitedAsync) :
                badFunc.Invoke(awaitedAsync);
        }

        /// <summary>
        /// Условие продолжающее действие с асинхронным выполнением
        /// </summary>      
        public static async Task<TResult> WhereContinueAsync<TSource, TResult>(this Task<TSource> @this, Func<TSource, bool> predicate,
                                                                               Func<TSource, Task<TResult>> okFunc,
                                                                               Func<TSource, Task<TResult>> badFunc)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            if (okFunc == null) throw new ArgumentNullException(nameof(okFunc));
            if (badFunc == null) throw new ArgumentNullException(nameof(badFunc));

            var awaitedAsync = await @this;

            return predicate(awaitedAsync)
                   ? await okFunc.Invoke(awaitedAsync)
                   : await badFunc.Invoke(awaitedAsync);
        }

        /// <summary>
        /// Обработка позитивного условия
        /// </summary>      
        public static async Task<TSource> WhereOkAsyncBind<TSource>(this TSource @this, Func<TSource, bool> predicate,
                                                                Func<TSource, Task<TSource>> okFunc) =>
               await @this.WhereContinueAsyncBind(predicate, okFunc, _ => new Task<TSource>(() => @this));

        /// <summary>
        /// Обработка позитивного условия
        /// </summary>      
        public static async Task<TSource> WhereOkAsync<TSource>(this Task<TSource> @this, Func<TSource, bool> predicate,
                                                                Func<TSource, TSource> okFunc)
        {
            var thisAwaited = await @this;
            return await @this.WhereContinueAsync(predicate, okFunc, _ => thisAwaited);
        }
        
        /// <summary>
        /// Обработка негативного условия
        /// </summary>      
        public static async Task<TSource> WhereBadAsyncBind<TSource>(this TSource @this, Func<TSource, bool> predicate,
                                                                 Func<TSource, Task<TSource>> badFunc) =>
            await @this.WhereContinueAsyncBind(predicate, _ => new Task<TSource>(() => @this), badFunc);

        /// <summary>
        /// Обработка негативного условия
        /// </summary>      
        public static async Task<TSource> WhereBadAsync<TSource>(this Task<TSource> @this, Func<TSource, bool> predicate,
                                                                 Func<TSource, TSource> badFunc)
        {
            var thisAwaited = await @this;
            return await @this.WhereContinueAsync(predicate, _ => thisAwaited, badFunc);
        }
    }
}
