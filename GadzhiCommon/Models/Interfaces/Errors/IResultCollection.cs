using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiCommon.Models.Interfaces.Errors
{
    /// <summary>
    /// Ответ с коллекцией
    /// </summary>
    public interface IResultCollection<T> : IResultValue<IEnumerable<T>>
    {
        /// <summary>
        /// Добавить ответ с коллекцией
        /// </summary>      
        IResultCollection<T> ConcatResult(IResultCollection<T> resultCollection);

        /// <summary>
        /// Добавить ответ со значением
        /// </summary>      
        IResultCollection<T> ConcatResultValue(IResultValue<T> resultValue);
    }
}
