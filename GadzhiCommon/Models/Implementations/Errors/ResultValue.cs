using GadzhiCommon.Models.Interfaces.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiCommon.Models.Implementations.Errors
{
    /// <summary>
    /// Базовый вариант ответа со значением
    /// </summary>
    public class ResultValue<TValue>: IResultValue<TValue>
    {
        public ResultValue(IErrorCommon error)
            : this(error.AsEnumerable()) { }

        public ResultValue(IEnumerable<IErrorCommon> errors)
        {
            if (errors == null) throw new ArgumentNullException(nameof(errors));
            if (!ValidateCollection(errors)) throw new NullReferenceException(nameof(errors));

            Errors = errors;
        }

        public ResultValue(TValue value)
          : this(value, Enumerable.Empty<IErrorCommon>()) { }

        public ResultValue(TValue value, IEnumerable<IErrorCommon> errors) 
            :this (errors)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));           

            Value = value;           
        }

        /// <summary>
        /// Список значений
        /// </summary>
        public TValue Value { get; }

        /// <summary>
        /// Список ошибок
        /// </summary>
        public IEnumerable<IErrorCommon> Errors { get; }

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
        public IResultValue<TValue> ConcatErrors(IEnumerable<IErrorCommon> errors) =>
            errors != null && ValidateCollection(errors) ?
            new ResultValue<TValue>(Value, Errors.Union(errors)) :
            this;

        /// <summary>
        /// Проверить ошибки на корретность
        /// </summary>      
        protected bool ValidateCollection<T>(IEnumerable<T> collection) =>
            collection?.All(t => t != null) == true;
    }
}
