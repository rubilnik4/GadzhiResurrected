using GadzhiApplicationCommon.Models.Interfaces.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiApplicationCommon.Models.Implementation.Errors
{
    /// <summary>
    /// Базовый вариант ответа
    /// </summary>
    public class ResultApplicationValue<TValue> : ResultApplication, IResultApplicationValue<TValue>
    {
        public ResultApplicationValue()
            : base() { }

        public ResultApplicationValue(IErrorApplication errorApplication)
            : base(errorApplication.AsEnumerable()) { }

        public ResultApplicationValue(IEnumerable<IErrorApplication> errorsApplication)
           : base(errorsApplication) { }

        public ResultApplicationValue(TValue value)
          : this(value, Enumerable.Empty<IErrorApplication>()) { }

        public ResultApplicationValue(TValue value, IEnumerable<IErrorApplication> errorsApplication)
            : base(errorsApplication)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            Value = value;
        }

        /// <summary>
        /// Список значений
        /// </summary>
        public TValue Value { get; }
    }
}
