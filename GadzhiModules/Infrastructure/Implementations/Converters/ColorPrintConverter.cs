using System.Collections.Generic;
using System.Linq;
using GadzhiCommon.Enums.FilesConvert;

namespace GadzhiModules.Infrastructure.Implementations.Converters
{
    /// <summary>
    /// Преобразование типа цвета в строку
    /// </summary>
    public static class ColorPrintConverter
    {
        /// <summary>
        /// Словарь цвета печати в строковом значении
        /// </summary>
        public static IReadOnlyDictionary<ColorPrint, string> ColorPrintString =>
            new Dictionary<ColorPrint, string>
            {
                { ColorPrint.BlackAndWhite, "Черно-белый" },
                { ColorPrint.GrayScale, "Градации серого" },
                { ColorPrint.Color, "Цветной" },
            };

        /// <summary>
        /// Преобразовать цветовое значение в наименование цвета
        /// </summary>       
        public static string ColorPrintToString(ColorPrint colorPrint)
        {
            ColorPrintString.TryGetValue(colorPrint, out string colorPrintString);
            return colorPrintString;
        }

        /// <summary>
        /// Преобразовать наименование цвета в цветовое значение
        /// </summary>       
        public static ColorPrint ConvertStringToColorPrint(string colorPrint) =>
            ColorPrintString?.FirstOrDefault(color => color.Value == colorPrint).Key
            ?? ColorPrint.BlackAndWhite;
    }
}
