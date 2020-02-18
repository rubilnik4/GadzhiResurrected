using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Models.StampCollections
{
    /// <summary>
    /// Дополнительные параметры штампа
    /// </summary>
    public static class StampAdditionalParameters
    {
        /// <summary>
        /// Символ разделяющий параметры внутри аттрибута
        /// </summary>
        public static string SeparatorAttribute => "$@";

        /// <summary>
        /// Разделить значение аттрибута через символ-разделитель
        /// </summary>
        public static IList<string> SeparateAttributeValue(string valueField)
        {
            return valueField?.Split(new string[] { SeparatorAttribute },
                                     StringSplitOptions.None).
                               ToList() ??
                   new List<string>();
        }

        /// <summary>
        /// Коэффициент сжатия текстовых элементов в штампе
        /// </summary>
        public static double CompressionRatio => 0.9d;
    }
}
