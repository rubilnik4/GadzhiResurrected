using GadzhiCommon.Enums.FilesConvert;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GadzhiApplicationCommon.Models.Enums;

namespace GadzhiConverting.Models.Converters
{
    /// <summary>
    /// Конвертировать тип цвета печати в версию для приложения
    /// </summary>
    public static class ColorPrintToApplicationConverter
    {
        /// <summary>
        /// Конвертировать тип цвета печати в версию для приложения
        /// </summary>
        public static ColorPrintApplication ToApplication(ColorPrint colorPrint) =>
            Enum.TryParse(colorPrint.ToString(), true, out ColorPrintApplication colorPrintApplication) ?
                colorPrintApplication :
                throw new FormatException(nameof(colorPrint));
    }
}
