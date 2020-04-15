using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiCommon.Extentions
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
