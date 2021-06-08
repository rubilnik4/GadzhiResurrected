using System.Collections.Generic;
using System.Linq;
using GadzhiCommon.Enums.FilesConvert;

namespace GadzhiResurrected.Infrastructure.Implementations.Converters
{
    /// <summary>
    /// Преобразование типа цвета в строку
    /// </summary>
    public static class ColorPrintConverter
    {
        /// <summary>
        /// Словарь цвета печати в строковом значении
        /// </summary>
        public static IReadOnlyDictionary<ColorPrintType, string> ColorPrintTypesString =>
            new Dictionary<ColorPrintType, string>
            {
                { ColorPrintType.BlackAndWhite, "Черно-белый" },
                { ColorPrintType.GrayScale, "Градации серого" },
                { ColorPrintType.Color, "Цветной" },
            };

        /// <summary>
        /// Типы цветов для печати
        /// </summary>
        public static IReadOnlyCollection<string> ColorPrintsString =>
            ColorPrintTypesString.Select(color => color.Value).ToList();

        /// <summary>
        /// Преобразовать цветовое значение в наименование цвета
        /// </summary>       
        public static string ColorPrintToString(ColorPrintType colorPrintType)
        {
            ColorPrintTypesString.TryGetValue(colorPrintType, out string colorPrintString);
            return colorPrintString;
        }

        /// <summary>
        /// Преобразовать наименование цвета в цветовое значение
        /// </summary>       
        public static ColorPrintType ColorPrintFromString(string colorPrint) =>
            ColorPrintTypesString.FirstOrDefault(color => color.Value == colorPrint).Key;
    }
}
