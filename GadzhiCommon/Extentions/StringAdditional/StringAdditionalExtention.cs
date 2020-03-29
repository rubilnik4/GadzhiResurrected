using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiCommon.Extentions.StringAdditional
{
    /// <summary>
    /// Методы расширения для строк
    /// </summary>
    public static class StringAdditionalExtentions
    {
        /// <summary>
        /// Удалить часть строки
        /// </summary>       
        public static string TrimSubstring(this string stringOriginal, string substring)
        {
            if (substring != null && substring.Length > 0 &&
                stringOriginal?.StartsWith(substring, StringComparison.OrdinalIgnoreCase) == true)
            {
                return stringOriginal.Remove(0, substring.Length)?.Trim();
            }
            return stringOriginal;
        }

        /// <summary>
        /// Сделать первую букву заглавной
        /// </summary>        
        public static string FirstCharToUpper(this string input)
        {
            switch (input)
            {
                case null: return "";
                case "": return "";
                default:
                    return input.First().ToString(CultureInfo.CurrentCulture).
                                         ToUpper(CultureInfo.CurrentCulture) + input.Substring(1);
            }
        }

        /// <summary>
        /// Сделать первую букву маленькой
        /// </summary>        
        public static string FirstCharToLower(this string input)
        {
            switch (input)
            {
                case null: return "";
                case "": return "";
                default:
                    return input.First().ToString(CultureInfo.CurrentCulture).
                                         ToLower(CultureInfo.CurrentCulture) + input.Substring(1);
            }
        }

        /// <summary>
        /// Содержит ли подстроку без учета регистар
        /// </summary>        
        public static bool ContainsIgnoreCase(this string input, string substring) =>
            !String.IsNullOrWhiteSpace(substring) ?
            input?.IndexOf(substring, StringComparison.OrdinalIgnoreCase) > -1 :
            false;
    }

}
