using System.Collections.Generic;
using GadzhiCommon.Enums.ConvertingSettings;

namespace GadzhiResurrected.Infrastructure.Implementations.Converters
{
    /// <summary>
    /// Преобразование типа конвертирования PDF в строку
    /// </summary>
    public static class PdfNamingConverter
    {
        /// <summary>
        /// Словарь типа конвертирования PDF в строковом значении
        /// </summary>
        public static IReadOnlyDictionary<PdfNamingType, string> PdfNamingString =>
            new Dictionary<PdfNamingType, string>
            {
                { PdfNamingType.ByFile, "По имени файла" },
                { PdfNamingType.ByCode, "По основному шифру" },
                { PdfNamingType.BySheet, "По номеру листа" },
            };

        /// <summary>
        /// Преобразовать цветовое значение в наименование цвета
        /// </summary>       
        public static string ColorPrintToString(PdfNamingType pdfNamingType)
        {
            PdfNamingString.TryGetValue(pdfNamingType, out string pdfNamingString);
            return pdfNamingString;
        }
    }
}