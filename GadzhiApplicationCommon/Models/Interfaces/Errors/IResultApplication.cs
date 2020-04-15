using GadzhiApplicationCommon.Functional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiApplicationCommon.Models.Interfaces.Errors
{
    /// <summary>
    /// Возвращаемый тип модуля после конвертации с учетом ошибок 
    /// </summary>
    public interface IResultApplication : IResultValue<Unit>
    {
        /// <summary>
        /// Добавить ошибку. Вернуть новый объект
        /// </summary>      
        new IResultApplication ConcatErrors(IEnumerable<IErrorApplication> errors);

        /// <summary>
        /// Преобразовать в результирующий ответ с параметром
        /// </summary>      
        IResultValue<T> ToResultApplicationValue<T>();

        /// <summary>
        /// Выполнить отложенные функции
        /// </summary>
        new IResultApplication ExecuteLazy();
    }
}
