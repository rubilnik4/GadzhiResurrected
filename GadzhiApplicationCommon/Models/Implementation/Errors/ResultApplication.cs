using GadzhiApplicationCommon.Models.Interfaces.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiApplicationCommon.Models.Implementation.Errors
{
    /// <summary>
    /// Возвращаемый тип модуля после конвертации с учетом ошибок
    /// </summary>
    public class ResultApplication : IResultApplication
    {
        public ResultApplication()
        {
            ErrorsApplication = Enumerable.Empty<IErrorApplication>();
        }

        public ResultApplication(IErrorApplication errorApplication)
        {
            ErrorsApplication = errorApplication ?? throw new ArgumentNullException(nameof(errorApplication));
        }

        public ResultApplication(IEnumerable<IErrorApplication> errorsApplication)
        {
            if (errorsApplication == null) throw new ArgumentNullException(nameof(errorsApplication));
            if (!ValidateErrors(errorsApplication)) throw new NullReferenceException(nameof(errorsApplication));

            ErrorsApplication = errorsApplication;
        }

        /// <summary>
        /// Список ошибок
        /// </summary>
        public IEnumerable<IErrorApplication> ErrorsApplication { get; }

        /// <summary>
        /// Присутствуют ли ошибки
        /// </summary>
        public bool HasErrors => ErrorsApplication.Any();

        /// <summary>
        /// Отсуствие ошибок
        /// </summary>
        public bool OkStatus => !HasErrors;

        /// <summary>
        /// Добавить ошибку. Вернуть новый объект
        /// </summary>      
        public IResultApplication ConcatResult(IResultApplication resultApplication) =>
            resultApplication != null ?
            new ResultApplication(ErrorsApplication.Union(resultApplication.ErrorsApplication)) :
            this;

        /// <summary>
        /// Проверить ошибки на корретность
        /// </summary>      
        private bool ValidateErrors(IEnumerable<IErrorApplication> errorsApplication) =>
            errorsApplication?.All(error => error != null) == true;
    }
}
