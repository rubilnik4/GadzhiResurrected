using System;
using System.Collections.Generic;
using System.Linq;
using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiApplicationCommon.Models.Implementation.LibraryData;
using GadzhiApplicationCommon.Models.Implementation.StampCollections.StampPartial;
using GadzhiWord.Word.Interfaces.Word.Elements;

namespace GadzhiWord.Models.Implementations.StampCollections.StampPartial
{
    /// <summary>
    /// Штамп. Базовый вариант
    /// </summary>
    public abstract partial class StampWord : Stamp
    {
        protected StampWord(StampSettingsWord stampSettingsWord, SignaturesSearching signaturesSearching, ITableElementWord tableStamp)
            : base(stampSettingsWord, signaturesSearching)
        {
            TableStamp = tableStamp ?? throw new ArgumentNullException(nameof(tableStamp));
            PaperSize = stampSettingsWord?.PaperSize ?? throw new ArgumentNullException(nameof(stampSettingsWord));
            Orientation = stampSettingsWord.Orientation;
        }

        /// <summary>
        /// Элемент таблица
        /// </summary>
        protected ITableElementWord TableStamp { get; }

        /// <summary>
        /// Формат
        /// </summary>
        public override string PaperSize { get; }

        /// <summary>
        /// Тип расположения штампа
        /// </summary>
        public override StampOrientationType Orientation { get; }

        /// <summary>
        /// Наименование
        /// </summary>
        public override string Name => $"{StampMarkersWord.StampTypeToString[StampType]}";
    }
}
