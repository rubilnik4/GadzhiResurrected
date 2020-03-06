using System;
using System.Collections.Generic;
using System.Linq;

namespace GadzhiMicrostation.Models.Implementations.StampCollections
{
    /// <summary>
    /// Дополнительные параметры штампа
    /// </summary>
    public static class StampAdditionalParameters
    {  
        /// <summary>
        /// Имя библиотеки с подписями
        /// </summary>
        public static string SignatureLibraryName => "Signature.cel";

        /// <summary>
        /// Имя библиотеки со штампами
        /// </summary>
        public static string StampLibraryName => "Stamp.cel";

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
        public static double CompressionRatioText => 0.9d;

        /// <summary>
        /// Коэффициент сжатия текстовых полей в штампе
        /// </summary>
        public static double CompressionRatioTextNode => 0.97d;

        /// <summary>
        /// Коэффициент сдвига ячейки подписи от базовой точки к нижнему левому углу
        /// </summary>
        public static double SignatureRatioMoveFromOriginToLow => 0.8d;
    }
}
