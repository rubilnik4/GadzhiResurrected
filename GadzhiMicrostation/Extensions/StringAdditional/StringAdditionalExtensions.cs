using System;
using System.Globalization;
using System.Linq;

namespace GadzhiMicrostation.Extensions.StringAdditional
{
    /// <summary>
    /// Методы расширения для строк
    /// </summary>
    public static class StringAdditionalExtensions
    {
        /// <summary>
        /// Удалить часть строки
        /// </summary>       
        public static string TrimSubstring(this string stringOriginal, string substring) =>

             (!String.IsNullOrEmpty(substring) &&
              stringOriginal?.StartsWith(substring, StringComparison.OrdinalIgnoreCase) == true)
                 ? stringOriginal.Remove(0, substring.Length).Trim()
                 : stringOriginal;

        /// <summary>
        /// Сделать первую букву заглавной
        /// </summary>        
        public static string FirstCharToUpper(this string input) =>
            input switch
            {
                null => String.Empty,
                "" => String.Empty,
                _ => input.FirstOrDefault().ToString(CultureInfo.CurrentCulture).ToUpper(CultureInfo.CurrentCulture) +
                     input.Substring(1)
            };

        /// <summary>
        /// Сделать первую букву маленькой
        /// </summary>        
        public static string FirstCharToLower(this string input) =>
            input switch
            {
                null => String.Empty,
                "" => String.Empty,
                _ => input.First().ToString().ToLowerCaseCurrentCulture() +
                     input.Substring(1)
            };

        /// <summary>
        /// Содержит ли подстроку без учета регистра
        /// </summary>        
        public static bool ContainsIgnoreCase(this string input, string substring) =>
            !String.IsNullOrEmpty(substring) && 
            input?.IndexOf(substring, StringComparison.OrdinalIgnoreCase) > -1;

        /// <summary>
        /// Является ли строка пустой
        /// </summary>       
        public static bool IsNullOrWhiteSpace(this string text) => String.IsNullOrEmpty(text?.Trim());

        /// <summary>
        /// Перевести строку в нижний регистр с учетом текущего языка
        /// </summary>
        public static string ToLowerCaseCurrentCulture(this string input) => input?.ToLower(CultureInfo.CurrentCulture)
                                                                             ?? String.Empty;
    }
}
