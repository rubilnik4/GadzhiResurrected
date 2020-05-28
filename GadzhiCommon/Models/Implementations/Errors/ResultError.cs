using GadzhiCommon.Models.Interfaces.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GadzhiCommon.Models.Implementations.Functional;

namespace GadzhiCommon.Models.Implementations.Errors
{
    /// <summary>
    /// Базовый вариант ответа
    /// </summary>
    public class ResultError : ResultValue<Unit>, IResultError
    {
        public ResultError()
            : this(Enumerable.Empty<IErrorCommon>()) { }

        public ResultError(IErrorCommon errorConverting)
            : this(errorConverting.AsEnumerable()) { }

        public ResultError(IEnumerable<IErrorCommon> errorsConverting)
            : base(errorsConverting) { }

        /// <summary>
        /// Добавить ошибку
        /// </summary>      
        public new IResultError ConcatErrors(IEnumerable<IErrorCommon> errors) => 
            new ResultError(base.ConcatErrors(errors).Errors);

        /// <summary>
        /// Преобразовать в результирующий ответ с параметром
        /// </summary>      
        public IResultValue<T> ToResultValue<T>(T value) => new ResultValue<T>(value, Errors);
    }
}
