using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GadzhiCommon.Models.Implementations.Functional;

namespace GadzhiCommon.Models.Interfaces.Errors
{
    /// <summary>
    /// Базовый вариант ответа
    /// </summary>
    public interface IResultError : IResultValue<Unit>
    {
        /// <summary>
        /// Добавить ошибку
        /// </summary>      
        new IResultError ConcatErrors(IEnumerable<IErrorCommon> errors);

        /// <summary>
        /// Преобразовать в результирующий ответ с параметром
        /// </summary>      
        IResultValue<T> ToResultValue<T>(T value);
    }
}
