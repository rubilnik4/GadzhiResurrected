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
    public class ResultConvertingValue<TValue> : ResultConverting, IResultConvertingValue<TValue>
    {
        public ResultConvertingValue()
            : base() { }

        public ResultConvertingValue(IErrorConverting error)
            : base(error.AsEnumerable()) { }

        public ResultConvertingValue(IEnumerable<IErrorConverting> errors)
           : base(errors) { }

        public ResultConvertingValue(TValue value)
          : this(value, Enumerable.Empty<IErrorConverting>()) { }

        public ResultConvertingValue(TValue value, IEnumerable<IErrorConverting> errors)
            : base(errors)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            Value = value;
        }

        /// <summary>
        /// Список значений
        /// </summary>
        public TValue Value { get; }

        /// <summary>
        /// Добавить ответ
        /// </summary>      
        public virtual IResultConvertingValue<TValue> ConcatResult(IResultConvertingValue<TValue> resultConverting) =>
            resultConverting != null ?
            new ResultConvertingValue<TValue>(resultConverting.Value, resultConverting.Errors) :
            this;

        /// <summary>
        /// Добавить ошибку
        /// </summary>      
        public virtual IResultConvertingValue<TValue> ConcatErrors(IEnumerable<IErrorConverting> errors) =>
            errors != null && Validate(errors) ?
            new ResultConvertingValue<TValue>(Value, Errors.Union(errors)) :
            this;

    }
}
