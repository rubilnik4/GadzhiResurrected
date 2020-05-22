using GadzhiApplicationCommon.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GadzhiCommon.Extensions.Collection;

// ReSharper disable All

namespace GadzhiWord.Models.Implementations.StampCollections
{
    /// <summary>
    /// Дополнительные параметры штампа
    /// </summary>
    public static class AdditionalSettingsWord
    {
        /// <summary>
        /// Тип штампа в строковом обозначении
        /// </summary>
        public static IReadOnlyDictionary<StampType, string> StampTypeToString => new Dictionary<StampType, string>()
        {
            { StampType.Main, "Основной"},
            { StampType.Additional, "Дополнительный"},
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
                                                            UnionNotNull(MarkersAdditionalStamp).
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
            "Изм."            
        };
    }
}
