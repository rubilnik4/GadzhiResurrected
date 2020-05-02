using GadzhiCommon.Models.Interfaces.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GadzhiCommon.Extensions.Collection;

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
            if (!ValidateCollection(collectionList)) throw new NullReferenceException(nameof(collection));

            InitializeValue(collectionList, errorNull);
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
            new ResultCollection<T>(resultValue.Value != null ?
                                        Value.Append(resultValue.Value) :
                                        Value,
                                    Errors.UnionNotNull(resultValue.Errors)) :
            throw new ArgumentNullException(nameof(resultValue));

        /// <summary>
        /// Добавить значение
        /// </summary>       
        public IResultCollection<T> ConcatValue(T value) =>
            value != null ?
            new ResultCollection<T>(Value.Append(value), Errors) :
            throw new ArgumentNullException(nameof(value));

        /// <summary>
        /// Выполнить отложенные функции
        /// </summary>
        public new IResultCollection<T> Execute() => new ResultCollection<T>(Value.ToList(), Errors.ToList());
    }
}
