using System;
using System.Globalization;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Infrastructure.Implementations.Converters.Errors;

namespace GadzhiModules.Modules.GadzhiConvertingModule.ViewModels.Tabs.FilesErrorsViewModelItems
{
    /// <summary>
    /// Представление ошибки с ее характеристиками
    /// </summary>
    public class FileErrorViewModelItem: IFormattable
    {
        public FileErrorViewModelItem( string fileName, FileConvertErrorType errorType, string errorDescription)
        {
            if (String.IsNullOrWhiteSpace(fileName)) throw new ArgumentNullException(nameof(fileName));

            FileName = fileName;
            ErrorType = errorType;
            ErrorDescription = errorDescription ?? throw new ArgumentNullException(nameof(errorDescription));
        }

        /// <summary>
        /// Имя файла
        /// </summary>
        public string FileName { get; }

        /// <summary>
        /// Тип ошибки
        /// </summary>
        public FileConvertErrorType ErrorType { get; }

        /// <summary>
        /// Тип ошибки в строковом значении
        /// </summary>
        public string ErrorTypeString => ConverterErrorType.ErrorTypeToString(ErrorType);

        /// <summary>
        /// Описание ошибки
        /// </summary>
        public string ErrorDescription { get; }

        #region IFormattable Support
        public override string ToString() => ToString(String.Empty, CultureInfo.CurrentCulture);

        public string ToString(string format, IFormatProvider formatProvider) => ErrorTypeString;
        #endregion
    }
}