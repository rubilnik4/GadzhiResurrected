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
    public class ResultConverting: IResultConverting
    {
        public ResultConverting()
        {
            Errors = Enumerable.Empty<IErrorConverting>();           
        }

        public ResultConverting(IErrorConverting errorConverting)
            : this(errorConverting.AsEnumerable()) { }      
      
        public ResultConverting(IEnumerable<IErrorConverting> errorsConverting)
        {
            if (errorsConverting == null) throw new ArgumentNullException(nameof(errorsConverting));
            if (!Validate(errorsConverting)) throw new NullReferenceException(nameof(errorsConverting));
           
            Errors = errorsConverting;
        }
      
        /// <summary>
        /// Список ошибок
        /// </summary>
        public IEnumerable<IErrorConverting> Errors { get; }

        /// <summary>
        /// Присутствуют ли ошибки
        /// </summary>
        public bool HasErrors => Errors.Any();

        /// <summary>
        /// Отсуствие ошибок
        /// </summary>
        public bool OkStatus => !HasErrors;

        /// <summary>
        /// Добавить ошибку. Вернуть новый объект
        /// </summary>      
        public virtual IResultConverting ConcatResult(IResultConverting resultConverting) =>
            resultConverting != null ?
            new ResultConverting(Errors.Union(resultConverting.Errors)) :
            this;

        /// <summary>
        /// Проверить ошибки на корретность
        /// </summary>      
        protected bool Validate<T>(IEnumerable<T> collection) where T : class =>
            collection?.All(t => t != null) == true;
    }
}
