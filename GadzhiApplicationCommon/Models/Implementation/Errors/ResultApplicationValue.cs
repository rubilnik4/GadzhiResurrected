using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiMicrostation.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiApplicationCommon.Models.Implementation.Errors
{
    /// <summary>
    /// Базовый вариант ответа
    /// </summary>
    public class ResultApplicationValue<TValue> : IResultApplicationValue<TValue>
    {
        public ResultApplicationValue(IErrorApplication error)
           : this(error.AsEnumerable()) { }

        public ResultApplicationValue(IEnumerable<IErrorApplication> errors)
        {
            if (errors == null) throw new ArgumentNullException(nameof(errors));
            if (!ValidateCollection(errors)) throw new NullReferenceException(nameof(errors));

            Errors = errors;
        }

        public ResultApplicationValue(TValue value, IErrorApplication errorNull = null)
          : this(value, Enumerable.Empty<IErrorApplication>(), errorNull) { }

        public ResultApplicationValue(TValue value, IEnumerable<IErrorApplication> errors, IErrorApplication errorNull = null)
            : this(errors)
        {
            if (value == null && errorNull != null)
            {
                Errors = Errors.Concat(errorNull);
            }
            else if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            Value = value;
        }

        /// <summary>
        /// Список значений
        /// </summary>
        public TValue Value { get; }

        /// <summary>
        /// Список ошибок
        /// </summary>
        public IEnumerable<IErrorApplication> Errors { get; }

        /// <summary>
        /// Присутствуют ли ошибки
        /// </summary>
        public bool HasErrors => Errors.Any();

        /// <summary>
        /// Отсуствие ошибок
        /// </summary>
        public bool OkStatus => !HasErrors;

        /// <summary>
        /// Добавить ошибку
        /// </summary>      
        public IResultApplicationValue<TValue> ConcatErrors(IEnumerable<IErrorApplication> errors) =>
            errors != null && ValidateCollection(errors) ?
            new ResultApplicationValue<TValue>(Value, Errors.Union(errors)) :
            this;

        /// <summary>
        /// Проверить ошибки на корретность
        /// </summary>      
        protected bool ValidateCollection<T>(IEnumerable<T> collection) =>
            collection?.All(t => t != null) == true;
    }
}
