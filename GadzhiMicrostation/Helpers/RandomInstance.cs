using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Helpers
{
    /// <summary>
    /// Генератор случайных чисел
    /// </summary>
    public static class RandomInstance
    {
        /// <summary>
        /// Генератор случайных чисел
        /// </summary>
        private static readonly Random Random = new Random();

        /// <summary>
        /// Блокировка потока
        /// </summary>
        private static readonly object SyncLock = new object();

        /// <summary>
        /// Получить случайное число до максимального предела
        /// </summary>
        public static int RandomNumber(int max) => RandomNumber(0, max);

        /// <summary>
        /// Получить случайное число от минимума до максимума
        /// </summary>
        public static int RandomNumber(int min, int max)
        {
            lock (SyncLock)
            {
                return Random.Next(min, max);
            }
        }
    }
}
