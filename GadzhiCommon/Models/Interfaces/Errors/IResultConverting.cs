using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiCommon.Models.Interfaces.Errors
{
    /// <summary>
    /// Базовый вариант ответа
    /// </summary>
    public interface IResultConverting
    {      
        /// <summary>
        /// Список ошибок
        /// </summary>
        IEnumerable<IErrorConverting> Errors { get; }

        /// <summary>
        /// Присутствуют ли ошибки
        /// </summary>
        bool HasErrors { get; }

        /// <summary>
        /// Отсуствие ошибок
        /// </summary>
        bool OkStatus { get; }

        /// <summary>
        /// Добавить ошибку. Вернуть новый объект
        /// </summary>      
        IResultConverting ConcatResult(IResultConverting errorConverting);
    }
}
