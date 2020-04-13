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
    public interface IResult: IResultValue<Unit>
    {
        /// <summary>
        /// Добавить ошибку
        /// </summary>      
        new IResult ConcatErrors(IEnumerable<IErrorCommon> errors);
    }
}
