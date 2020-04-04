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

        public ResultApplicationValue(IErrorApplication error)
            : base(error.AsEnumerable()) { }

        public ResultApplicationValue(IEnumerable<IErrorApplication> errors)
           : base(errors) { }

        public ResultApplicationValue(TValue value)
          : this(value, Enumerable.Empty<IErrorApplication>()) { }

        public ResultApplicationValue(TValue value, IEnumerable<IErrorApplication> errors)
            : base(errors)
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
