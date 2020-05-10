using GadzhiCommon.Models.Interfaces.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GadzhiCommon.Extensions.Functional;

namespace GadzhiCommon.Models.Implementations.Errors
{
    /// <summary>
    /// Базовый вариант ответа со значением
    /// </summary>
    public class ResultValue<TValue> : IResultValue<TValue>
    {
        public ResultValue(IErrorCommon error)
            : this(error.AsEnumerable()) { }

        public ResultValue(IEnumerable<IErrorCommon> errors)
        {
            var errorCollection = errors?.ToList() ?? throw new ArgumentNullException(nameof(errors));
            if (!ValidateCollection(errorCollection)) throw new NullReferenceException(nameof(errors));

            Errors = errorCollection;
        }

        public ResultValue(TValue value, IErrorCommon errorNull = null)
          : this(value, Enumerable.Empty<IErrorCommon>(), errorNull) { }

        public ResultValue(TValue value, IEnumerable<IErrorCommon> errors, IErrorCommon errorNull = null)
            : this(errors)
        {
            Errors = value switch
            {
                null when errorNull != null => Errors.Concat(errorNull),
                null => throw new ArgumentNullException(nameof(value)),
                _ => Errors
            };

            Value = value;
        }

        /// <summary>
        /// Список значений
        /// </summary>
        public TValue Value { get; protected set; }

        /// <summary>
        /// Список ошибок
        /// </summary>
        public IEnumerable<IErrorCommon> Errors { get; protected set; }

        /// <summary>
        /// Присутствуют ли ошибки
        /// </summary>
        public bool HasErrors => Errors.Any();

        /// <summary>
        /// Отсутствие ошибок
        /// </summary>
        public bool OkStatus => !HasErrors;

        /// <summary>
        /// Добавить ошибку
        /// </summary>      
        public IResultValue<TValue> ConcatErrors(IEnumerable<IErrorCommon> errors) =>
            errors != null ?
            Value.WhereContinue(value => value != null,
                okFunc: value => new ResultValue<TValue>(value, Errors.Union(errors)),
                badFunc: value => new ResultValue<TValue>(Errors.Union(errors))) :
            this;

        /// <summary>
        /// Преобразовать в результирующий тип
        /// </summary>
        public IResultError ToResult() => new ResultError(Errors);

        /// <summary>
        /// Выполнить отложенные функции
        /// </summary>
        public IResultValue<TValue> Execute() => new ResultValue<TValue>(Value, Errors.ToList());

        /// <summary>
        /// Проверить ошибки на корректность
        /// </summary>      
        protected static bool ValidateCollection<T>(IEnumerable<T> collection) =>
            collection?.All(t => t != null) == true;
    }
}
