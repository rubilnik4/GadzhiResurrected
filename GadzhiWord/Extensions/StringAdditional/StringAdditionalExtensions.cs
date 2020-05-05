using GadzhiWord.Word.Implementations.Converters;

namespace GadzhiWord.Extensions.StringAdditional
{
    /// <summary>
    /// Методы расширения для строковых значений в классах библиотеки Word
    /// </summary>
    public static class StringAdditionalExtensions
    {        
        /// <summary>
        /// Обработать текст ячейки
        /// </summary>        
        public static string PrepareCellTextToCompare(this string cellText) => TextPrepare.PrepareCellTextToCompare(cellText);
    }
}
