using GadzhiCommon.Enums.FilesConvert;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GadzhiModules.Helpers.Converters
{
    public static class ColorPrintConverter
    {
        /// <summary>
        /// Словарь цвета печати в строком значении
        /// </summary>
        public static IReadOnlyDictionary<ColorPrint, string> ColorPrintToString =>
            new Dictionary<ColorPrint, string>
            {
                { ColorPrint.BlackAndWhite, "Черно-белый" },
                { ColorPrint.GrayScale, "Градации серого" },
                { ColorPrint.Color, "Цветной" },
            };

        /// <summary>
        /// Преобразовать цветовое значение в наименование цвета
        /// </summary>       
        public static string ConvertColorPrintToString(ColorPrint colorPrint)
        {
            string colorPrintString = String.Empty;
            ColorPrintToString?.TryGetValue(colorPrint, out colorPrintString);

            return colorPrintString;
        }

        /// <summary>
        /// Преобразовать наименование цвета в цветовое значение
        /// </summary>       
        public static ColorPrint ConvertStringToColorPrint(string colorPrint)
        {
            ColorPrint colorPrintOut = ColorPrintToString?.FirstOrDefault(color => color.Value == colorPrint).Key ??
                                                           ColorPrint.BlackAndWhite;

            return colorPrintOut;
        }
    }
}
