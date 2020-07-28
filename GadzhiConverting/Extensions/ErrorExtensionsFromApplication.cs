using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiCommon.Models.Interfaces.Errors;
using GadzhiConverting.Models.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiConverting.Extensions
{
    /// <summary>
    /// Преобразование внутреннего класса ошибок библиотеки в основной
    /// </summary>
    public static class ErrorExtensionsFromApplication
    {
        /// <summary>
        /// Преобразовать внутренний класс ошибок библиотеки в основной
        /// </summary>
        public static IErrorCommon ToErrorConverting(this IErrorApplication errorApplication) =>
            ErrorApplicationConverter.ToErrorConverting(errorApplication);

        /// <summary>
        /// Преобразовать внутренний класс ошибок библиотеки из основного
        /// </summary>
        public static IErrorApplication ToErrorApplication(this IErrorCommon errorCommon) =>
            ErrorApplicationConverter.ToErrorApplication(errorCommon);

        /// <summary>
        /// Преобразовать внутренние классы ошибок библиотеки в основные
        /// </summary>
        public static IEnumerable<IErrorCommon> ToErrorsConverting(this IEnumerable<IErrorApplication> errorsApplication) =>
            errorsApplication?.Select(error => error.ToErrorConverting())
            ?? throw new ArgumentNullException(nameof(errorsApplication));

        /// <summary>
        /// Преобразовать внутренние классы ошибок библиотеки из основного
        /// </summary>
        public static IEnumerable<IErrorApplication> ToErrorsApplication(this IEnumerable<IErrorCommon> errorsCommon) =>
            errorsCommon?.Select(error => error.ToErrorApplication())
            ?? throw new ArgumentNullException(nameof(errorsCommon));
    }   
}
