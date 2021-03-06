﻿using GadzhiCommon.Models.Interfaces.Errors;
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
    public class ResultCollection<T> : ResultValue<IReadOnlyList<T>>, IResultCollection<T>
    {
        public ResultCollection()
          : this(Enumerable.Empty<T>()) { }

        public ResultCollection(IErrorCommon error)
            : this(Enumerable.Empty<T>(), error.AsEnumerable()) { }

        public ResultCollection(IEnumerable<IErrorCommon> errors)
           : base(new List<T>(), errors) { }

        public ResultCollection(IEnumerable<T> collection)
          : this(collection, Enumerable.Empty<IErrorCommon>()) { }

        public ResultCollection(IEnumerable<T> collection, IEnumerable<IErrorCommon> errors, IErrorCommon errorNull = null)
            : base(errors)
        {
            var collectionList = collection?.ToList();

            Errors = collectionList switch
            {
                null when errorNull != null => Errors.Concat(errorNull).ToList(),
                null => throw new ArgumentNullException(nameof(collection)),
                _ => Errors
            };

            Value = collectionList ?? new List<T>();
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
                                        ? Value.Concat(new List<T> { resultValue.Value }) 
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

    }
}
