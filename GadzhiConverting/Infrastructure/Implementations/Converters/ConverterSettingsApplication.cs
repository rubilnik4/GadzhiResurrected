using System;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Implementation.FilesConvert;
using GadzhiCommon.Enums.ConvertingSettings;
using GadzhiConverting.Models.Interfaces.FilesConvert;

namespace GadzhiConverting.Infrastructure.Implementations.Converters
{
    /// <summary>
    /// Преобразование параметров конвертации в класс модуля конвертирования
    /// </summary>
    public static class ConverterSettingsApplication
    {
        /// <summary>
        /// Преобразовать параметры конвертации в класс модуля конвертирования
        /// </summary>
        public static ConvertingSettingsApp ToConvertingSettingsApplication(IConvertingSettings convertingSettings) =>
            new ConvertingSettingsApp(convertingSettings.PersonId,
                                      ToPdfNamingTypeApplication(convertingSettings.PdfNamingType),
                                      convertingSettings.UseDefaultSignature);

        /// <summary>
        /// Преобразовать принцип именования PDF в класс модуля конвертирования
        /// </summary>
        public static PdfNamingTypeApplication ToPdfNamingTypeApplication(PdfNamingType pdfNamingType) =>
            pdfNamingType switch
            {
                PdfNamingType.ByFile => PdfNamingTypeApplication.ByFile,
                PdfNamingType.ByCode => PdfNamingTypeApplication.BySheet,
                PdfNamingType.BySheet => PdfNamingTypeApplication.ByStamp,
                _ => throw new ArgumentOutOfRangeException(nameof(pdfNamingType), pdfNamingType, null)
            };
    }
}
