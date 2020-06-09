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
            { StampType.Full, "Основной"},
            { StampType.Shortened, "Сокращенный"},
            { StampType.ChangeNotice, "Согласование извещения"},
        };

        /// <summary>
        /// Маркеры основного штампа
        /// </summary>
        public static IReadOnlyList<string> MarkersFullStamp => new List<string>()
        {
            "Разраб",
            "Исполн",
            "Составил",
        };

        /// <summary>
        /// Маркеры извещения изменений
        /// </summary>
        public static IReadOnlyList<string> MarkersChangeNoticeStamp => new List<string>()
        {
            "Изм.внес",
        };


        /// <summary>
        /// Маркеры дополнительного штампа
        /// </summary>
        public static IReadOnlyList<string> MarkersAdditionalStamp => new List<string>()
        {
            "Лист",
        };

        /// <summary>
        /// Маркеры таблицы согласования исполнителей
        /// </summary>
        public static IReadOnlyList<string> MarkersApprovalPerformanceTable => new List<string>()
        {
            "Список исполнителей"
        };

        /// <summary>
        /// Маркеры штампа
        /// </summary>
        public static IReadOnlyList<string> MarkersStamp => MarkersFullStamp.
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
            "Н.контр",
            "Изм.внес",
            "Утв.",
        };

        /// <summary>
        /// Маркеры типа действия в строке с ответственным лицом с ответственностью внутри отдела
        /// </summary>
        public static IReadOnlyList<string> MarkersActionTypeChief => new List<string>()
        {
             "ГИП"
        };

        /// <summary>
        /// Маркеры типа действия в строке с ответственным лицом для извещений с изменениями
        /// </summary>
        public static IReadOnlyList<string> MarkersActionTypeChangeNotice => new List<string>()
        {
            "Изм.внес",
            "Составил",
            "Утв.",
            "ГИП",
        };

        /// <summary>
        /// Маркеры штампа для извещений с изменениями
        /// </summary>
        public static IReadOnlyList<string> MarkersApprovalChangeStamp => new List<string>()
        {
            "Согласовано",
        };

        /// <summary>
        /// Маркеры типа действия в строке с ответственным лицом для извещений с изменениями
        /// </summary>
        public static IReadOnlyList<string> MarkersApprovalChange => new List<string>()
        {
            "Н.контр",
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
            var hasChangeMarkers = false;

            foreach (var cell in tableWord.CellsElementWord)
            {
                if (CheckFieldType.IsFieldFullCode(cell, tableWord)) hasFullCode = true;
                if (MarkersAdditionalStamp.MarkerContain(cell.Text)) hasShortMarker = true;
                if (MarkersFullStamp.MarkerContain(cell.Text)) hasMainMarkers = true;
                if (MarkersChangeNoticeStamp.MarkerContain(cell.Text)) hasChangeMarkers = true;
            }

            return hasFullCode switch
            {
                true when hasMainMarkers => StampType.Full,
                true when hasShortMarker => StampType.Shortened,
                _ when hasChangeMarkers => StampType.ChangeNotice,
                false => StampType.Unknown,
                _ => StampType.Unknown,
            };
        }
    }
}
