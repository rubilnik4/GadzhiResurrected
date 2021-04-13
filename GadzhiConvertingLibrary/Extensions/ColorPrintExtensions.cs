using GadzhiApplicationCommon.Models.Enums;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiConvertingLibrary.Models.Converters;

namespace GadzhiConvertingLibrary.Extensions
{
    /// <summary>
    /// Методы расширения для цвета печати
    /// </summary>
    public static class ColorPrintExtensions
    {
        /// <summary>
        /// Конвертировать тип цвета печати в версию для приложения
        /// </summary>
        public static ColorPrintApplication ToApplication(this ColorPrint colorPrint) => 
            ColorPrintToApplicationConverter.ToApplication(colorPrint);      
    }
}
