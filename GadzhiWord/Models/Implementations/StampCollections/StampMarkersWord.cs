using GadzhiApplicationCommon.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiWord.Models.Implementations.StampCollections.Fields;
using GadzhiWord.Word.Interfaces.Word.Elements;
using GadzhiWord.Extensions.Word;

namespace GadzhiWord.Models.Implementations.StampCollections
{
    /// <summary>
    /// Дополнительные параметры штампа
    /// </summary>
    public static class StampMarkersWord
    {
        /// <summary>
        /// Тип штампа в строковом обозначении
        /// </summary>
        public static IReadOnlyDictionary<StampType, string> StampTypeToString => new Dictionary<StampType, string>()
        {
            { StampType.Main, "Основной"},
            { StampType.Shortened, "Сокращенный"},
        };

        /// <summary>
        /// Маркеры основного штампа
        /// </summary>
        public static IReadOnlyList<string> MarkersMainStamp => new List<string>()
        {
            "Разраб",
            "Исполн",
            "Составил",
        };

        /// <summary>
        /// Маркеры дополнительного штампа
        /// </summary>
        public static IReadOnlyList<string> MarkersAdditionalStamp => new List<string>()
        {
            "Лист",
        };

        /// <summary>
        /// Маркеры штампа
        /// </summary>
        public static IReadOnlyList<string> MarkersStamp => MarkersMainStamp.
                                                            Union(MarkersAdditionalStamp).
                                                            ToList();

        /// <summary>
        /// Маркеры типа действия в строке с ответственным лицом
        /// </summary>
        public static IReadOnlyList<string> MarkersActionType =>
            MarkersActionTypeDepartment.Concat(MarkersActionTypeChief).ToList();

        /// <summary>
        /// Маркеры типа действия в строке с ответственным лицом с ответственностью внутри отдела
        /// </summary>
        public static IReadOnlyList<string> MarkersActionTypeDepartment => new List<string>()
        {
            "Разраб",
            "Исполн",
            "Составил",
            "Проверил",
            "Вед.инж",
            "Нач.гр",
            "Гл.спец",
            "Нач.сек",
            "Нач.отд",
            "Зам.Нач.отд",
            "Н.конт",
        };

        /// <summary>
        /// Маркеры типа действия в строке с ответственным лицом с ответственностью внутри отдела
        /// </summary>
        public static IReadOnlyList<string> MarkersActionTypeChief => new List<string>()
        {
             "ГИП"
        };

        /// <summary>
        /// Маркеры строки заголовка изменений
        /// </summary>
        public static IReadOnlyList<string> MarkersChangeHeader => new List<string>()
        {
            "Изм"
        };

        /// <summary>
        /// Содержит ли коллекция маркеров указанное поле
        /// </summary>        
        public static bool MarkerContain(IEnumerable<string> markerCollection, string cellText) =>
            markerCollection?.Any(marker => cellText?.StartsWith(marker, StringComparison.CurrentCultureIgnoreCase) == true) == true;

        /// <summary>
        /// Проверить является ли таблица штампом и вернуть его тип
        /// </summary>
        public static StampType GetStampType(ITableElementWord tableWord)
        {
            var hasFullCode = false;
            var hasShortMarker = false;
            var hasMainMarkers = false;

            foreach (var cell in tableWord.CellsElementWord)
            {
                if (CheckFieldType.IsFieldFullCode(cell, tableWord)) hasFullCode = true;
                if (MarkersAdditionalStamp.MarkerContain(cell.Text)) hasShortMarker = true;
                if (MarkersMainStamp.MarkerContain(cell.Text)) hasMainMarkers = true;
            }

            return hasFullCode switch
            {
                true when hasMainMarkers => StampType.Main,
                true when hasShortMarker => StampType.Shortened,
                false => StampType.Unknown,
                _ => StampType.Unknown,
            };
        }
    }
}
