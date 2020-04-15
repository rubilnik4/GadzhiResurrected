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
    public class ResultApplication : ResultValue<Unit>, IResultApplication
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
            errors != null && ValidateCollection(errors) ?
            new ResultApplication(Errors.Union(Errors)) :
            this;

        /// <summary>
        /// Преобразовать в результирующий ответ с параметром
        /// </summary>      
        public IResultValue<T> ToResultApplicationValue<T>() => new ResultValue<T>(Errors);

        /// <summary>
        /// Выполнить отложенные функции
        /// </summary>
        public new IResultApplication ExecuteLazy() => new ResultApplication(Errors.ToList());
    }
}
