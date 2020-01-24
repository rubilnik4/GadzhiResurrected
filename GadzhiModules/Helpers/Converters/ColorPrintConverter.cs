using GadzhiModules.Modules.FilesConvertModule.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiModules.Helpers.Converters
{
    public static class ColorPrintConverter
    {
        /// <summary>
        /// Словарь цвета печати в строком значении
        /// </summary>
        public static IReadOnlyDictionary<ColorPrint, string> ColorPrintToString =
            new Dictionary<ColorPrint, string>
            {
                { ColorPrint.BlackAndWhite, "Черно-белый" },
                { ColorPrint.GrayScale, "Градации серого" },
                { ColorPrint.Color, "Цветной" },
            };

        public static string ConvertColorPrintToString (ColorPrint colorPrint)
        {
            string colorPrintString = String.Empty;
            ColorPrintToString?.TryGetValue(colorPrint, out colorPrintString);
           
            return colorPrintString;
        }

        public static ColorPrint ConvertColorPrintToColor(string colorPrint)
        {
            ColorPrint ColorPrintOut = ColorPrintToString?.FirstOrDefault(color => color.Value == colorPrint).Key ?? 
                                                           ColorPrint.BlackAndWhite;             

            return ColorPrintOut;
        }
    }
}
