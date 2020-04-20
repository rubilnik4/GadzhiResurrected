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
    public interface IResultApplication : IResultAppValue<Unit>
    {
        /// <summary>
        /// Добавить ошибку. Вернуть новый объект
        /// </summary>      
        new IResultApplication ConcatErrors(IEnumerable<IErrorApplication> errors);

        /// <summary>
        /// Преобразовать в результирующий ответ с параметром
        /// </summary>      
        IResultAppValue<T> ToResultApplicationValue<T>();

        /// <summary>
        /// Выполнить отложенные функции
        /// </summary>
        new IResultApplication ExecuteLazy();
    }
}
