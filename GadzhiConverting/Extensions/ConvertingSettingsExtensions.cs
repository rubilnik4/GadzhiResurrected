using System;
using GadzhiApplicationCommon.Models.Implementation.FilesConvert;
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
            (convertingSettings != null)
                ? new ConvertingSettingsApplication(convertingSettings.Department)
                : throw new ArgumentNullException(nameof(convertingSettings));
    }
}