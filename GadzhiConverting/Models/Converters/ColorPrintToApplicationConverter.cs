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
        public static ColorPrintApplication ConvertingToApplication(ColorPrint colorPrint) =>
            colorPrint switch
            {
                ColorPrint.BlackAndWhite => ColorPrintApplication.BlackAndWhite,
                ColorPrint.Color => ColorPrintApplication.Color,
                ColorPrint.GrayScale => ColorPrintApplication.GrayScale,
                _ => ColorPrintApplication.BlackAndWhite
            };
    }
}
