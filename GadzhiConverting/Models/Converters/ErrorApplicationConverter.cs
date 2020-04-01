using GadzhiApplicationCommon.Models.Interfaces;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiCommon.Models.Interfaces.Errors;
using GadzhiMicrostation.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public static IErrorConverting ToErrorConverting(IErrorApplication errorApplication) =>
            (errorApplication != null) ?
             new ErrorConverting(ToFileConvertErrorType(errorApplication.ErrorMicrostationType), errorApplication.ErrorDescription) :
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
