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
    public class Result : ResultValue<Unit>, IResult
    {
        public Result()
            : this(Enumerable.Empty<IErrorCommon>()) { }

        public Result(IErrorCommon errorConverting)
            : this(errorConverting.AsEnumerable()) { }

        public Result(IEnumerable<IErrorCommon> errorsConverting)
            : base(errorsConverting) { }

        /// <summary>
        /// Добавить ошибку
        /// </summary>      
        public new IResult ConcatErrors(IEnumerable<IErrorCommon> errors) =>
            errors != null && ValidateCollection(errors) ?
            new Result(Errors.Union(Errors)) :
            this;
    }
}
