using System.Collections.Generic;
using System.Linq;
using GadzhiCommon.Enums.FilesConvert;

namespace GadzhiModules.Infrastructure.Implementations.Converters
{
    /// <summary>
    /// Преобразования типа конвертации
    /// </summary>
    public static class ConvertingModeTypeConverter
    {
        /// <summary>
        /// Словарь цвета печати в строковом значении
        /// </summary>
        public static IReadOnlyDictionary<ConvertingModeType, string> ConvertingModeTypesString =>
            new Dictionary<ConvertingModeType, string>
            {
                { ConvertingModeType.All, "Все типы" },
                { ConvertingModeType.Pdf, "Только PDF" },
                { ConvertingModeType.Export, "Только DWG и XLS" },
            };

        /// <summary>
        /// Типы конвертации
        /// </summary>
        public static IReadOnlyCollection<string> ConvertingModeString =>
            ConvertingModeTypesString.Select(converting => converting.Value).ToList();

        /// <summary>
        /// Преобразовать тип конвертации в наименование
        /// </summary>       
        public static string ConvertingModeToString(ConvertingModeType convertingModeType)
        {
            ConvertingModeTypesString.TryGetValue(convertingModeType, out string convertingModeString);
            return convertingModeString;
        }

        /// <summary>
        /// Преобразовать наименование в тип конвертации
        /// </summary>       
        public static ConvertingModeType ConvertingModeFromString(string colorPrint) =>
            ConvertingModeTypesString.FirstOrDefault(convertingMode => convertingMode.Value == colorPrint).Key;
    }
}