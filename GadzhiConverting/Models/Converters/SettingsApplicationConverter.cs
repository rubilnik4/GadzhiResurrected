using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Implementation.FilesConvert;
using GadzhiCommon.Enums.ConvertingSettings;
using GadzhiConverting.Models.Interfaces.FilesConvert;

namespace GadzhiConverting.Models.Converters
{
    /// <summary>
    /// Преобразование параметров конвертации в класс модуля конвертирования
    /// </summary>
    public static class SettingsApplicationConverter
    {
        /// <summary>
        /// Преобразовать параметры конвертации в класс модуля конвертирования
        /// </summary>
        public static ConvertingSettingsApplication ToConvertingSettingsApplication(IConvertingSettings convertingSettings) =>
            (convertingSettings != null)
                ? new ConvertingSettingsApplication(convertingSettings.PersonId, ToPdfNamingTypeApplication(convertingSettings.PdfNamingType))
                : throw new ArgumentNullException(nameof(convertingSettings));

        /// <summary>
        /// Преобразовать принцип именования PDF в класс модуля конвертирования
        /// </summary>
        public static PdfNamingTypeApplication ToPdfNamingTypeApplication(PdfNamingType pdfNamingType) =>
            pdfNamingType switch
            {
                PdfNamingType.ByFile => PdfNamingTypeApplication.ByFile,
                PdfNamingType.ByStamp => PdfNamingTypeApplication.BySheet,
                PdfNamingType.BySheet => PdfNamingTypeApplication.ByStamp,
                _ => throw new ArgumentOutOfRangeException(nameof(pdfNamingType), pdfNamingType, null)
            };
    }
}
