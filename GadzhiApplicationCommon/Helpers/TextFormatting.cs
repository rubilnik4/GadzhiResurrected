using System;
using System.Linq;
using System.Text.RegularExpressions;
using GadzhiApplicationCommon.Extensions.Functional;

namespace GadzhiApplicationCommon.Helpers
{
    /// <summary>
    /// Класс для обработки строковых значений в модуле Word
    /// </summary>
    public static class TextFormatting
    {
        /// <summary>
        /// Удалить из текста артефакты Word и пробелы
        /// </summary>        
        public static string RemoveSpacesAndArtefacts(string cellText) =>
            Regex.Replace(RemoveArtefacts(cellText), @"\s+", "");

        /// <summary>
        /// Удалить из текста пробелы
        /// </summary>        
        public static string RemoveSpaces(string cellText) =>
             Regex.Replace(cellText, @"\s+", "");


        /// <summary>
        /// Удалить из текста артефакты Word
        /// </summary>        
        public static string RemoveArtefacts(string cellText) =>
            cellText?.Trim().
                      Replace("ё", "е").
                      Replace("c", "с").
                      Replace("у", "у").
                      Replace("o", "о").
                      Replace("..", ".").
                      Replace(((char)7).ToString(), String.Empty).
                      Replace(((char)10).ToString(), String.Empty).
                      Replace(((char)11).ToString(), String.Empty).
                      Replace(((char)13).ToString(), String.Empty).
                      Replace(((char)160).ToString(), String.Empty).
            Map(text => Regex.Replace(text, @"\s+", " "))
            ?? String.Empty;

        /// <summary>
        /// Получить слово максимальной длины
        /// </summary>
        public static string GetMaxLengthWord(string cellText) =>
            cellText?.Split().Max(text => text)
            ?? String.Empty;
    }
}
