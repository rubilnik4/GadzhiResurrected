using GadzhiApplicationCommon.Models.Interfaces.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiApplicationCommon.Models.Implementation.Errors
{
    /// <summary>
    /// Ответ с коллекцией
    /// </summary>
    public class ResultAppCollection<T> : ResultAppValue<IEnumerable<T>>, IResultAppCollection<T>
    {
        public ResultAppCollection()
          : base(Enumerable.Empty<T>()) { }

        public ResultAppCollection(IErrorApplication error)
            : base(Enumerable.Empty<T>(), error.AsEnumerable()) { }

        public ResultAppCollection(IEnumerable<IErrorApplication> errors)
           : base(Enumerable.Empty<T>(), errors) { }

        public ResultAppCollection(IEnumerable<T> collection)
          : this(collection, Enumerable.Empty<IErrorApplication>()) { }

        public ResultAppCollection(IEnumerable<T> collection, IEnumerable<IErrorApplication> errors)
            : base(collection, errors)
        {
            if (!ValidateCollection(collection)) throw new NullReferenceException(nameof(collection));
        }

        /// <summary>
        /// Добавить ответ с коллекцией
        /// </summary>      
        public IResultAppCollection<T> ConcatResult(IResultAppCollection<T> resultCollection) =>
            resultCollection != null ?
            new ResultAppCollection<T>(Value.Union(resultCollection.Value),
                                    Errors.Union(resultCollection.Errors ?? Enumerable.Empty<IErrorApplication>())) :
            this;

        /// <summary>
        /// Добавить ответ со значением
        /// </summary>      
        public IResultAppCollection<T> ConcatResultValue(IResultAppValue<T> resultValue) =>
            resultValue != null ?
            ConcatValue(resultValue.Value) :
            throw new ArgumentNullException(nameof(resultValue));

        /// <summary>
        /// Добавить значение
        /// </summary>       
        public IResultAppCollection<T> ConcatValue(T value) =>
            value != null ?
            new ResultAppCollection<T>(Value.Concat(new List<T>() { value }),
                                       Errors.Union(Errors ?? Enumerable.Empty<IErrorApplication>())) :
            throw new ArgumentNullException(nameof(value));

        /// <summary>
        /// Выполнить отложенные функции
        /// </summary>
        public IResultAppCollection<T> Execute() => new ResultAppCollection<T>(Value.ToList(), Errors.ToList());
    }
}
