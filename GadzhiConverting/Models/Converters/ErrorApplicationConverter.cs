using System;
using GadzhiApplicationCommon.Models.Enums;
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
             new ErrorCommon(ToFileConvertErrorType(errorApplication.ErrorMicrostationType), errorApplication.ErrorDescription) :
             throw new ArgumentNullException(nameof(errorApplication));

        /// <summary>
        /// Преобразовать внутренний тип ошибок библиотеки в основной тип
        /// </summary>       
        public static FileConvertErrorType ToFileConvertErrorType(ErrorApplicationType errorApplicationType) =>
            Enum.TryParse(errorApplicationType.ToString(), true, out FileConvertErrorType fileConvertErrorType) ?
            fileConvertErrorType :
            throw new FormatException(nameof(FileConvertErrorType));
    }
}
