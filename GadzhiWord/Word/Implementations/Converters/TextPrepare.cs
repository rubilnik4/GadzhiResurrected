using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GadzhiWord.Word.Implementations.Converters
{
    /// <summary>
    /// Класс для обработки строковых значений в модуле Word
    /// </summary>
    public static class TextPrepare
    {
        /// <summary>
        /// Обработать текст ячейки
        /// </summary>        
        public static string PrepareCellTextToCompare(string cellText)
        {
            string preparedText = cellText?.Replace("ё", "е").
                                      Replace("c", "с").
                                      Replace("у", "у").
                                      Replace("o", "о").
                                      Replace("..", ".").
                                      Replace(((char) 7).ToString(), String.Empty).
                                      Replace(((char) 10).ToString(), String.Empty).
                                      Replace(((char) 11).ToString(), String.Empty).
                                      Replace(((char) 13).ToString(), String.Empty).
                                      Replace(((char) 160).ToString(), String.Empty).
                                      Replace(((char) 176).ToString(), String.Empty)
                                  ?? String.Empty;

            return Regex.Replace(preparedText, @"\s+", "");
        }

    }
}
