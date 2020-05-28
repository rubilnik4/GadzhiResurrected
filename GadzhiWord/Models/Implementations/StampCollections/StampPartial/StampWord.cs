using System;
using System.Collections.Generic;
using System.Linq;
using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiApplicationCommon.Models.Implementation.LibraryData;
using GadzhiApplicationCommon.Models.Implementation.StampCollections;
using GadzhiApplicationCommon.Models.Implementation.StampCollections.StampPartial;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Fields;
using GadzhiWord.Models.Implementations.StampCollections.Fields;
using GadzhiWord.Models.Interfaces.StampCollections;
using GadzhiWord.Word.Interfaces.Elements;

namespace GadzhiWord.Models.Implementations.StampCollections.StampPartial
{
    /// <summary>
    /// Штамп. Базовый вариант
    /// </summary>
    public abstract partial class StampWord : Stamp
    {
        protected StampWord(StampSettingsWord stampSettingsWord, SignaturesSearching signaturesSearching, ITableElement tableStamp)
            : base(stampSettingsWord, signaturesSearching)
        {
            TableStamp = tableStamp ?? throw new ArgumentNullException(nameof(tableStamp));
            PaperSize = stampSettingsWord?.PaperSize ?? throw new ArgumentNullException(nameof(stampSettingsWord));
            Orientation = stampSettingsWord.Orientation;
        }

        /// <summary>
        /// Элемент таблица
        /// </summary>
        protected ITableElement TableStamp { get; }

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
        public override string Name => $"{AdditionalSettingsWord.StampTypeToString[StampType]}";
    }
}
