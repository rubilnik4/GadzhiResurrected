using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiApplicationCommon.Models.Interfaces.Errors
{
    /// <summary>
    /// Ответ с коллекцией
    /// </summary>
    public interface IResultAppCollection<T> : IResultAppValue<IEnumerable<T>>
    {
        /// <summary>
        /// Добавить ответ с коллекцией
        /// </summary>      
        IResultAppCollection<T> ConcatResult(IResultAppCollection<T> resultCollection);

        /// <summary>
        /// Добавить ответ со значением
        /// </summary>      
        IResultAppCollection<T> ConcatResultValue(IResultAppValue<T> resultValue);

        /// <summary>
        /// Выполнить отложенные функции
        /// </summary>
        new IResultAppCollection<T> Execute();
    }
}
