using System;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiCommon.Enums.FilesConvert;

namespace GadzhiConvertingLibrary.Models.Converters
{
    /// <summary>
    /// Конвертировать тип цвета печати в версию для приложения
    /// </summary>
    public static class ColorPrintToApplicationConverter
    {
        /// <summary>
        /// Конвертировать тип цвета печати в версию для приложения
        /// </summary>
        public static ColorPrintApplication ToApplication(ColorPrintType colorPrintType) =>
            Enum.TryParse(colorPrintType.ToString(), true, out ColorPrintApplication colorPrintApplication) ?
                colorPrintApplication :
                throw new FormatException(nameof(colorPrintType));
    }
}
