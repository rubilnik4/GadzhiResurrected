using GadzhiCommon.Models.Interfaces.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GadzhiCommon.Extensions.Collection;
using GadzhiCommon.Extensions.Functional;

namespace GadzhiCommon.Models.Implementations.Errors
{
    /// <summary>
    /// Ответ с коллекцией
    /// </summary>
    public class ResultCollection<T> : ResultValue<IEnumerable<T>>, IResultCollection<T>
    {
        public ResultCollection()
          : base(Enumerable.Empty<T>()) { }

        public ResultCollection(IErrorCommon error)
            : base(Enumerable.Empty<T>(), error.AsEnumerable()) { }

        public ResultCollection(IEnumerable<IErrorCommon> errors)
           : base(Enumerable.Empty<T>(), errors) { }

        public ResultCollection(IEnumerable<T> collection)
          : this(collection, Enumerable.Empty<IErrorCommon>()) { }

        public ResultCollection(IEnumerable<T> collection, IEnumerable<IErrorCommon> errors, IErrorCommon errorNull = null)
            : base(errors)
        {
            var collectionList = collection?.ToList();

            Errors = collectionList?.Count switch
            {
                null when errorNull != null => Errors.Concat(errorNull),
                0 when errorNull != null => Errors.Concat(errorNull),
                null => throw new ArgumentNullException(nameof(collection)),
                0 => throw new ArgumentNullException(nameof(collection)),
                _ => Errors
            };

            Value = collectionList;
        }

        /// <summary>
        /// Добавить ответ с коллекцией
        /// </summary>      
        public IResultCollection<T> ConcatResult(IResultCollection<T> resultCollection) =>
            resultCollection != null ?
            new ResultCollection<T>(Value.UnionNotNull(resultCollection.Value),
                                    Errors.Union(resultCollection.Errors ?? Enumerable.Empty<IErrorCommon>())) :
            throw new ArgumentNullException(nameof(resultCollection));

        /// <summary>
        /// Добавить ответ со значением
        /// </summary>      
        public IResultCollection<T> ConcatResultValue(IResultValue<T> resultValue) =>
            resultValue != null ?
            new ResultCollection<T>(resultValue.Value != null 
                                        ? Value.Append(resultValue.Value) 
                                        : Value,
                                    Errors.Union(resultValue.Errors)) :
            throw new ArgumentNullException(nameof(resultValue));

        /// <summary>
        /// Добавить значение
        /// </summary>       
        public IResultCollection<T> ConcatValue(T value) =>
            value != null
                ? new ResultCollection<T>(Value.Concat(new List<T>() { value }), Errors)
                : throw new ArgumentNullException(nameof(value));

        /// <summary>
        /// Добавить значения
        /// </summary>       
        public IResultCollection<T> ConcatValues(IEnumerable<T> values) =>
            values.ToList().
            Map(valueCollection => ValidateCollection(valueCollection)
                                    ? new ResultCollection<T>(Value.Concat(valueCollection), Errors)
                                    : throw new NullReferenceException(nameof(valueCollection)));

        /// <summary>
        /// Выполнить отложенные функции
        /// </summary>
        public new IResultCollection<T> Execute() => new ResultCollection<T>(Value.ToList(), Errors.ToList());
    }
}
