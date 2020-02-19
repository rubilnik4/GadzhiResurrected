using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
        public static double CompressionRatioText => 0.9d;

        /// <summary>
        /// Коэффициент сжатия текстовых полей в штампе
        /// </summary>
        public static double CompressionRatioTextNode => 0.97d;

        /// <summary>
        /// Папка с ресурсами и библиотеками
        /// </summary>
        public static string MicrostationDataFolder => AppDomain.CurrentDomain.BaseDirectory + "MicrostationData\\";

        /// <summary>
        /// Имя библиотеки с подписями
        /// </summary>
        public static string SignatureLibraryPath => MicrostationDataFolder + "Signature.cel";
       
        /// <summary>
        /// Имя библиотеки со штампами
        /// </summary>
        public static string StampLibraryPath => MicrostationDataFolder + "Stamp.cel";

    }
}
