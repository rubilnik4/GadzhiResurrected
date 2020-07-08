using System;
using System.Threading.Tasks;

namespace GadzhiCommon.Extensions.Functional
{
    /// <summary>
    /// Методы расширения для преобразования типов
    /// </summary>
    public static class MapAsyncExtensions
    { 
        /// <summary>
        /// Преобразование типов с помощью функции
        /// </summary>       
        public static async Task<TResult> MapAsync<TSource, TResult>(this Task<TSource> @this, Func<TSource, TResult> func)
        {
            if (func == null) throw new ArgumentNullException(nameof(func));
            var awaited = await @this;
            return func.Invoke(awaited);
        }

        /// <summary>
        /// Связывание типов с помощью функции
        /// </summary>       
        public static async Task<TResult> MapBindAsync<TSource, TResult>(this Task<TSource> @this, Func<TSource, Task<TResult>> func)
        {
            if (func == null) throw new ArgumentNullException(nameof(func));
            var awaited = await @this;
            return await func.Invoke(awaited);
        }

        /// <summary>
        /// Выполнить асинхронное действие
        /// </summary>       
        public static async Task VoidAsync(this Task @this, Func<Task> action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));
            await @this;
            await action.Invoke();
        }

        /// <summary>
        /// Выполнить асинхронное действие
        /// </summary>       
        public static async Task<T> VoidAsync<T>(this Task<T> @this, Action<T> action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));

            var awaitedThis = await @this;
            action.Invoke(awaitedThis);

            return awaitedThis;
        }

        /// <summary>
        /// Выполнить асинхронное действие, вернуть тот же тип
        /// </summary>       
        public static async Task<T> VoidBindAsync<T>(this T @this, Func<T, Task> action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));
            await action.Invoke(@this);
            return @this;
        }

        /// <summary>
        /// Выполнить асинхронное действие, вернуть тот же тип
        /// </summary>       
        public static async Task<T> VoidBindAsync<T>(this Task<T> @this, Func<T, Task> action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));
            var awaited = await @this;
            await action.Invoke(awaited);
            return awaited;
        }
    }
}
