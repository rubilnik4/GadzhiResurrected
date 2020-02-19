using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Extensions.String
{
    /// <summary>
    /// Методы расширения для строк
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Удалить часть строки
        /// </summary>       
        public static string TrimSubstring(this string stringOriginal, string substring)
        {            
            if (substring != null && substring.Length > 0 &&
                stringOriginal?.StartsWith(substring) == true)
            {
                return stringOriginal.Remove(substring.Length);
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
                default: return input.First().ToString().ToUpper() + input.Substring(1);
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
                case "": return  "";
                default: return input.First().ToString().ToLower() + input.Substring(1);
            }
        }
    }
}
