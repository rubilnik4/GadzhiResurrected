using GadzhiCommon.Helpers.Strings;

namespace GadzhiCommon.Extensions.StringAdditional
{
    /// <summary>
    /// Методы расширения для текста
    /// </summary>
    public static class TextExtensions
    {
        /// <summary>
        /// Удалить из текста артефакты Word и пробелы
        /// </summary>     
        public static string RemoveSpacesAndArtefacts(this string cellText) => TextFormatting.RemoveSpacesAndArtefacts(cellText);

        /// <summary>
        /// Удалить из текста пробелы
        /// </summary>        
        public static string RemoveSpaces(this string cellText) => TextFormatting.RemoveSpaces(cellText);

        /// <summary>
        /// Удалить из текста артефакты Word
        /// </summary>        
        public static string RemoveArtefacts(this string cellText) => TextFormatting.RemoveArtefacts(cellText);
    }
}