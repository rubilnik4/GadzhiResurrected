using GadzhiApplicationCommon.Functional;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiApplicationCommon.Models.Implementation.Errors
{
    /// <summary>
    /// Возвращаемый тип модуля после конвертации с учетом ошибок
    /// </summary>
    public class ResultApplication : ResultAppValue<Unit>, IResultApplication
    {
        public ResultApplication()
            : this(Enumerable.Empty<IErrorApplication>()) { }

        public ResultApplication(IErrorApplication errorConverting)
            : this(errorConverting.AsEnumerable()) { }

        public ResultApplication(IEnumerable<IErrorApplication> errorsConverting)
            : base(errorsConverting) { }

        /// <summary>
        /// Добавить ошибку
        /// </summary>      
        public new IResultApplication ConcatErrors(IEnumerable<IErrorApplication> errors) =>
            new ResultApplication(base.ConcatErrors(errors).Errors);

        /// <summary>
        /// Преобразовать в результирующий ответ с параметром
        /// </summary>      
        public IResultAppValue<T> ToResultApplicationValue<T>(T value) => new ResultAppValue<T>(value, Errors);
    }
}
