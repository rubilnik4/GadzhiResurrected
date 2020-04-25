using GadzhiCommon.Functional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        IResultValue<T> ToResultValue<T>();

        /// <summary>
        /// Выполнить отложенные функции
        /// </summary>
        new IResultError Execute();
    }
}
