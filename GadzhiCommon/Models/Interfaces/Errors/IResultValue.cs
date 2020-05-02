using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiCommon.Models.Interfaces.Errors
{
    /// <summary>
    /// Базовый вариант ответа со значением
    /// </summary>
    public interface IResultValue<out TValue>
    {
        /// <summary>
        /// Список значений
        /// </summary>
        TValue Value { get; }

        /// <summary>
        /// Список ошибок
        /// </summary>
        IEnumerable<IErrorCommon> Errors { get; }

        /// <summary>
        /// Присутствуют ли ошибки
        /// </summary>
        bool HasErrors { get; }

        /// <summary>
        /// Отсутствие ошибок
        /// </summary>
        bool OkStatus { get; }

        /// <summary>
        /// Добавить ошибку
        /// </summary>      
        IResultValue<TValue> ConcatErrors(IEnumerable<IErrorCommon> errors);

        /// <summary>
        /// Преобразовать в результирующий тип
        /// </summary>
        IResultError ToResult();

        /// <summary>
        /// Выполнить отложенные функции
        /// </summary>
        IResultValue<TValue> Execute();
    }
}
