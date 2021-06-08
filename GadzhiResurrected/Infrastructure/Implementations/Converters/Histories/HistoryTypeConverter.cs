using System.Collections.Generic;
using System.Linq;
using GadzhiResurrected.Modules.GadzhiConvertingModule.Models.Enums.Histories;

namespace GadzhiResurrected.Infrastructure.Implementations.Converters.Histories
{
    /// <summary>
    /// Преобразование типа истории в строку
    /// </summary>
    public static class HistoryTypeConverter
    {
        /// <summary>
        /// Словарь типа истории в строковом значении
        /// </summary>
        public static IReadOnlyDictionary<HistoryType, string> HistoryTypesString =>
            new Dictionary<HistoryType, string>
            {
                { HistoryType.Package, "Пакеты" },
                { HistoryType.File, "Файлы" },
            };

        /// <summary>
        /// Типы истории
        /// </summary>
        public static IReadOnlyCollection<string> HistoriesString =>
            HistoryTypesString.Select(history => history.Value).ToList();

        /// <summary>
        /// Преобразовать тип истории в наименование
        /// </summary>       
        public static string HistoryToString(HistoryType historyType)
        {
            HistoryTypesString.TryGetValue(historyType, out string historyString);
            return historyString;
        }

        /// <summary>
        /// Преобразовать наименование истории в тип
        /// </summary>       
        public static HistoryType HistoryFromString(string historyType) =>
            HistoryTypesString.FirstOrDefault(history => history.Value == historyType).Key;
    }
}