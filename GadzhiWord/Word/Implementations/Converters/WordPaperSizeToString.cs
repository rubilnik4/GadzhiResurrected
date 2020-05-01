using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiWord.Word.Implementations.Converters
{
    /// <summary>
    /// Преобразование формата листа Word в строковое значение
    /// </summary>
    public static class WordPaperSizeToString
    {
        /// <summary>
        /// Преобразовать формат листа в строковое значение
        /// </summary>
        public static string ConvertingPaperSizeToString(WdPaperSize paperSize) =>
            paperSize switch
            {
                WdPaperSize.wdPaperA3 => "A3",
                WdPaperSize.wdPaperA4 => "A4",
                _ => String.Empty
            };
    }
}
