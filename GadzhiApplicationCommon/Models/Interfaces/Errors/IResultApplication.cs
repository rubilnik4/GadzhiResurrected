using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiApplicationCommon.Models.Interfaces.Errors
{
    /// <summary>
    /// Возвращаемый тип модуля после конвертации с учетом ошибок 
    /// </summary>
    public interface IResultApplication
    {
        /// <summary>
        /// Список ошибок
        /// </summary>
        IEnumerable<IErrorApplication> ErrorsApplication { get; }

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
        IResultApplication ConcatResult(IResultApplication errorApplication);
    }
}
