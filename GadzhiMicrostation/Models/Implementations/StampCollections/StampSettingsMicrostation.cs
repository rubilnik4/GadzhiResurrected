using System;
using System.Collections.Generic;
using System.Linq;

namespace GadzhiMicrostation.Models.Implementations.StampCollections
{
    /// <summary>
    /// Дополнительные параметры штампа
    /// </summary>
    public static class StampSettingsMicrostation
    {
        /// <summary>
        /// Символ разделяющий параметры внутри атрибута
        /// </summary>
        public static string SeparatorAttribute => "$@";

        /// <summary>
        /// Разделить значение атрибута через символ-разделитель
        /// </summary>
        public static IList<string> SeparateAttributeValue(string valueField) =>
            valueField?.
            Split(new[] { SeparatorAttribute }, StringSplitOptions.None).
            ToList()
            ?? new List<string>();

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
