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

        public ResultConvertingValue(IErrorConverting errorConverting)
            : base(errorConverting.AsEnumerable()) { }

        public ResultConvertingValue(IEnumerable<IErrorConverting> errorsConverting)
           : base(errorsConverting) { }

        public ResultConvertingValue(TValue value)
          : this(value, Enumerable.Empty<IErrorConverting>()) { }

        public ResultConvertingValue(TValue value, IEnumerable<IErrorConverting> errorsConverting)
            : base(errorsConverting)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            ValueConverting = value;
        }

        /// <summary>
        /// Список значений
        /// </summary>
        public TValue ValueConverting { get; }

        /// <summary>
        /// Добавить ошибку. Вернуть новый объект
        /// </summary>      
        public virtual IResultConvertingValue<TValue> ConcatResult(IResultConvertingValue<TValue> resultConverting) =>
            resultConverting != null ?
            new ResultConvertingValue<TValue>(, ErrorsConverting.Union(resultConverting.ErrorsConverting)) :
            this;
    }
}
