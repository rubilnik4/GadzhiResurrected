using GadzhiWord.Word.Implementations.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiWord.Extension.StringAdditional
{
    /// <summary>
    /// Методы расширения для строковых значений в классах библиотеки Word
    /// </summary>
    public static class StringAdditionalExtensions
    {        
        /// <summary>
        /// Обработать текст ячейки
        /// </summary>        
        public static string PrepareCellTextToCompare(this string cellText) =>
            TextPrepare.PrepareCellTextToCompare(cellText);
    }
}
