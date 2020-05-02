using System;

namespace GadzhiCommon.Extensions
{
    /// <summary>
    /// Методы расширения для нулевых типов
    /// </summary>
    public static class NullExtensions
    {
        /// <summary>
        /// Проверка переменной на null и вызов исключения
        /// </summary>      
        public static T NonNull<T>(this T nullable) where T : class
        {
            if (nullable == null) throw new ArgumentNullException(typeof(T).ToString());
            return nullable;
        }
    }
}
