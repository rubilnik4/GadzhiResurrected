﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GadzhiApplicationCommon.Functional;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Implementation.LibraryData;
using GadzhiApplicationCommon.Models.Implementation.StampCollections;
using GadzhiApplicationCommon.Models.Interfaces.ApplicationLibrary.Document;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiWord.Models.Interfaces.StampCollections;
using GadzhiWord.Word.Interfaces.Elements;

namespace GadzhiWord.Models.Implementations.StampCollections
{
    /// <summary>
    /// Штамп. Базовый вариант
    /// </summary>
    public abstract class StampWord : Stamp
    {
        protected StampWord(ITableElement tableStamp, StampSettingsWord stampSettingsWord,
                            SignaturesLibrarySearching signaturesLibrarySearching)
            : base(stampSettingsWord)
        {
            TableStamp = tableStamp ?? throw new ArgumentNullException(nameof(tableStamp));
            PaperSize = stampSettingsWord?.PaperSize ?? throw new ArgumentNullException(nameof(stampSettingsWord));
            Orientation = stampSettingsWord.Orientation;
            SignaturesLibrarySearching = signaturesLibrarySearching ?? throw new ArgumentNullException(nameof(signaturesLibrarySearching));
        }

        /// <summary>
        /// Элемент таблица
        /// </summary>
        protected ITableElement TableStamp { get; }

        /// <summary>
        /// Поиск имен с идентификатором и подписью
        /// </summary>
        protected SignaturesLibrarySearching SignaturesLibrarySearching { get; }

        /// <summary>
        /// Наименование
        /// </summary>
        public override string Name => $"{AdditionalSettingsWord.StampTypeToString[StampType]}";

        /// <summary>
        /// Формат
        /// </summary>
        public override string PaperSize { get; }

        /// <summary>
        /// Тип расположения штампа
        /// </summary>
        public override OrientationType Orientation { get; }

        /// <summary>
        /// Получить поля штампа
        /// </summary>
        private IReadOnlyList<IStampFieldWord> _fieldsStamp;

        /// <summary>
        /// Получить поля штампа
        /// </summary>
        protected IReadOnlyList<IStampFieldWord> FieldsStamp => _fieldsStamp ??= GetFields();

        /// <summary>
        /// Получить поля штампа
        /// </summary>
        private IReadOnlyList<IStampFieldWord> GetFields() =>
            TableStamp?.CellsElementWord?.
                        Where(cell => !String.IsNullOrWhiteSpace(cell.Text)).
                        Select(cell => new StampFieldWord(cell, CheckFieldType.GetStampFieldType(cell, TableStamp))).
                        Where(field => field.StampFieldType != StampFieldType.Unknown).
                        ToList();

        /// <summary>
        /// Вписать текстовые поля в рамки
        /// </summary>
        public override IEnumerable<bool> CompressFieldsRanges() => Enumerable.Empty<bool>();
    }
}
