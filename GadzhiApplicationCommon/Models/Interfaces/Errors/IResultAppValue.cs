using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiApplicationCommon.Models.Interfaces.Errors
{
    /// <summary>
    /// Базовый вариант ответа
    /// </summary>
    public interface IResultAppValue<out TValue>
    {
        /// <summary>
        /// Список значений
        /// </summary>
        TValue Value { get; }

        /// <summary>
        /// Список ошибок
        /// </summary>
        IEnumerable<IErrorApplication> Errors { get; }

        /// <summary>
        /// Присутствуют ли ошибки
        /// </summary>
        bool HasErrors { get; }

        /// <summary>
        /// Отсуствие ошибок
        /// </summary>
        bool OkStatus { get; }

        /// <summary>
        /// Добавить ошибку
        /// </summary>      
        IResultAppValue<TValue> ConcatErrors(IEnumerable<IErrorApplication> errors);

        /// <summary>
        /// Преобразовать в результирующий тип
        /// </summary>
        IResultApplication ToResultApplication();

        /// <summary>
        /// Выполнить отложенные функции
        /// </summary>
        IResultAppValue<TValue> ExecuteLazy();
    }
}
