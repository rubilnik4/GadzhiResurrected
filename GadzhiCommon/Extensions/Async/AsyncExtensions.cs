using System.Threading.Tasks;

namespace GadzhiCommon.Extensions.Async
{
    /// <summary>
    /// Методы расширения для асинхронных методов
    /// </summary>
    public static class AsyncExtensions
    {
        /// <summary>
        /// Выполнить асинхронный метод и вернуть новую задачу
        /// </summary>
        public static async Task<T> WatchTaskAsync<T>(this Task<T> task) => await task;
    }
}