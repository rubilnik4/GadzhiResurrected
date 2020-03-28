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
        private static readonly Random random = new Random();

        /// <summary>
        /// Блокировка потока
        /// </summary>
        private static readonly object syncLock = new object();

        /// <summary>
        /// Получить случайное число
        /// </summary>
        public static int RandomNumber(int max) => random.Next(0, max);      

        /// <summary>
        /// Получить случайное число
        /// </summary>
        public static int RandomNumber(int min, int max)
        {
            lock (syncLock)
            {
                return random.Next(min, max);
            }
        }
    }
}
