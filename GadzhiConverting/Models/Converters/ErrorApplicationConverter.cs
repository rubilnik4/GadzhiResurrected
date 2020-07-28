using System;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiCommon.Models.Interfaces.Errors;

namespace GadzhiConverting.Models.Converters
{
    /// <summary>
    /// Преобразование внутреннего класса ошибок библиотеки в основной
    /// </summary>
    public static class ErrorApplicationConverter
    {
        /// <summary>
        /// Преобразовать внутренний класс ошибок библиотеки в основной
        /// </summary>
        public static IErrorCommon ToErrorConverting(IErrorApplication errorApplication) =>
            (errorApplication != null) ?
             new ErrorCommon(ToFileConvertErrorType(errorApplication.ErrorMicrostationType), errorApplication.Description) :
             throw new ArgumentNullException(nameof(errorApplication));

        /// <summary>
        /// Преобразовать внутренний класс ошибок библиотеки из основного типа
        /// </summary>
        public static IErrorApplication ToErrorApplication(IErrorCommon errorApplication) =>
            (errorApplication != null) ?
             new ErrorApplication(ToErrorApplicationType(errorApplication.ErrorConvertingType), errorApplication.Description) :
             throw new ArgumentNullException(nameof(errorApplication));

        /// <summary>
        /// Преобразовать внутренний тип ошибок библиотеки в основной тип
        /// </summary>       
        public static ErrorConvertingType ToFileConvertErrorType(ErrorApplicationType errorApplicationType) =>
            Enum.TryParse(errorApplicationType.ToString(), true, out ErrorConvertingType errorConvertingType) ?
            errorConvertingType :
            throw new FormatException(nameof(ErrorConvertingType));

        /// <summary>
        /// Преобразовать внутренний тип ошибок библиотеки из основного типа
        /// </summary>       
        public static ErrorApplicationType ToErrorApplicationType(ErrorConvertingType errorConvertingType) =>
            Enum.TryParse(errorConvertingType.ToString(), true, out ErrorApplicationType errorApplicationType) ?
            errorApplicationType :
            throw new FormatException(nameof(ErrorConvertingType));
    }
}
