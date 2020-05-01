using GadzhiApplicationCommon.Extensions.Collection;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiCommon.Extentions.Collection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// ReSharper disable All

namespace GadzhiWord.Models.Implementations.StampCollections
{
    /// <summary>
    /// Дополнительные параметры штампа
    /// </summary>
    public static class StampSettingsWord
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
        public static IReadOnlyList<string> MarkersActionTypeSignature => new List<string>()
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
