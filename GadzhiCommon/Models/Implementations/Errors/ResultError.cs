using GadzhiCommon.Functional;
using GadzhiCommon.Models.Interfaces.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            errors != null && ValidateCollection(errors) ?
            new ResultError(Errors.Union(Errors)) :
            this;

        /// <summary>
        /// Преобразовать в результирующий ответ с параметром
        /// </summary>      
        public IResultValue<T> ToResultValue<T>(T value) => new ResultValue<T>(value, Errors);

        /// <summary>
        /// Выполнить отложенные функции
        /// </summary>
        public new IResultError Execute() => new ResultError(Errors.ToList());
    }
}
