using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiCommon.Functional
{
    /// <summary>
    /// Методы для преобразования типов
    /// </summary>
    public static class MapFunctional
    {
        /// <summary>
        /// Преобразование типов с помощью функции
        /// </summary>    
        public static TResult Map<TSourceFirst, TSourceSecond, TResult>(TSourceFirst sourceFirst, TSourceSecond sourceSecond,
                                                                        Func<TSourceFirst, TSourceSecond, TResult> func) =>
            func != null ?
            func(sourceFirst, sourceSecond) :
            throw new ArgumentNullException(nameof(func));
    }
}
