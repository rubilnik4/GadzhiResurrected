using System;
using System.Globalization;
using System.Linq;

namespace GadzhiCommon.Extensions.StringAdditional
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
                null => "",
                "" => "",
                _ => input.FirstOrDefault().ToString(CultureInfo.CurrentCulture).ToUpper(CultureInfo.CurrentCulture) +
                     input.Substring(1)
            };

        /// <summary>
        /// Сделать первую букву маленькой
        /// </summary>        
        public static string FirstCharToLower(this string input) =>
            input switch
            {
                null => "",
                "" => "",
                _ => input.FirstOrDefault().ToString(CultureInfo.CurrentCulture).ToLowerCaseCurrentCulture() +
                     input.Substring(1)
            };


        /// <summary>
        /// Содержит ли подстроку без учета регистр
        /// </summary>        
        public static bool ContainsIgnoreCase(this string input, string substring) =>
            !String.IsNullOrWhiteSpace(substring) &&
            input?.IndexOf(substring, StringComparison.OrdinalIgnoreCase) > -1;

        /// <summary>
        /// Перевести строку в нижний регистр с учетом текущего языка
        /// </summary>
        public static string ToLowerCaseCurrentCulture(this string input) => input?.ToLower(CultureInfo.CurrentCulture)
                                                                             ?? String.Empty;

        /// <summary>
        /// Перевести строку в верхний регистр с учетом текущего языка
        /// </summary>
        public static string ToUpperCaseCurrentCulture(this string input) => input?.ToUpper(CultureInfo.CurrentCulture)
                                                                             ?? String.Empty;

        /// <summary>
        /// Получить строку из массива или вернуть пустое значение
        /// </summary>
        public static string GetStringFromArrayOrEmpty(this string[] input, int index) =>
            (input?.Length > index && index >= 0)
                ? input[index]
                : String.Empty;

        /// <summary>
        /// Преобразовать массив в строку начиная с индекса или вернуть пустое значение
        /// </summary>
        public static string JoinStringArrayFromIndexToEndOrEmpty(this string[] input, int startIndex) =>
            (input?.Length > startIndex && startIndex >= 0)
                ? String.Join(" ", input, startIndex, input.Length - startIndex)
                : String.Empty;
    }

}
