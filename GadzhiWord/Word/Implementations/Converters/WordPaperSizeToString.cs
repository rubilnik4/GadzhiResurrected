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
        public static string ConvertingPaperSizeToString(WdPaperSize paperSize)
        {
            switch (paperSize)
            {
                case WdPaperSize.wdPaperA3:
                    return "A3";
                case WdPaperSize.wdPaperA4:
                    return "A4";
            }
            return String.Empty;
        }
    }
}
