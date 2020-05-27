using System;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Implementation.FilesConvert;
using GadzhiCommon.Enums.ConvertingSettings;
using GadzhiConverting.Models.Converters;
using GadzhiConverting.Models.Interfaces.FilesConvert;

namespace GadzhiConverting.Extensions
{

    /// <summary>
    /// Методы расширения для параметров конвертации
    /// </summary>
    public static class ConvertingSettingsExtensions
    {
        /// <summary>
        /// Преобразовать параметры конвертации в класс модуля конвертирования
        /// </summary>
        public static ConvertingSettingsApplication ToApplication(this IConvertingSettings convertingSettings) =>
            SettingsApplicationConverter.ToConvertingSettingsApplication(convertingSettings);

        /// <summary>
        /// Преобразовать принцип именования PDF в класс модуля конвертирования
        /// </summary>
        public static PdfNamingTypeApplication ToApplication(this PdfNamingType pdfNamingType)=>
            SettingsApplicationConverter.ToPdfNamingTypeApplication(pdfNamingType);
    }
}