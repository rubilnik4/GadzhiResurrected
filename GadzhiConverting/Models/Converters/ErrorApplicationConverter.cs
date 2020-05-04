using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiCommon.Models.Interfaces.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GadzhiApplicationCommon.Models.Enums;

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
            Enum.TryParse(errorApplicationType.ToString(), out FileConvertErrorType fileConvertErrorType) ?
            fileConvertErrorType :
            throw new FormatException(nameof(FileConvertErrorType));
    }
}
