using GadzhiCommon.Enums.FilesConvert;
using GadzhiMicrostation.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiConverting.Models.Converters
{
    /// <summary>
    /// Конвертировать тип цвета печати в версию для приложения
    /// </summary>
    public static class ColorPrintToApplication
    {
        /// <summary>
        /// Конвертировать тип цвета печати в версию для приложения
        /// </summary>
        public static ColorPrintApplication ConvertingToApplication(ColorPrint colorPrint)
        {
            switch (colorPrint)
            {
                case ColorPrint.BlackAndWhite:
                    return ColorPrintApplication.BlackAndWhite;
                case ColorPrint.Color:
                    return ColorPrintApplication.Color;
                case ColorPrint.GrayScale:
                    return ColorPrintApplication.GrayScale;
                default:
                    return ColorPrintApplication.BlackAndWhite;
            }
        }
    }
}
