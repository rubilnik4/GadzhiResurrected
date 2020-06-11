using System;
using System.Collections.Generic;
using System.Linq;
using GadzhiApplicationCommon.Extensions.Functional;
using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiWord.Models.Implementations.StampCollections.Fields;
using GadzhiWord.Models.Interfaces.StampCollections.Fields;
using GadzhiWord.Models.Interfaces.StampCollections.StampPartial;
using GadzhiWord.Word.Implementations.Word.Elements;
using GadzhiWord.Word.Interfaces.Word.Elements;

namespace GadzhiWord.Models.Implementations.StampCollections.StampPartial
{
    /// <summary>
    /// Подкласс для работа с внутренними полями штампа
    /// </summary>
    public partial class StampWord: IStampFieldsWord
    {
        /// <summary>
        /// Получить поля штампа
        /// </summary>
        private IReadOnlyList<IStampTextFieldWord> _fieldsStamp;

        /// <summary>
        /// Получить поля штампа
        /// </summary>
        protected IReadOnlyList<IStampTextFieldWord> FieldsStamp => _fieldsStamp ??= GetFields();

        /// <summary>
        /// Вписать текстовые поля в рамки
        /// </summary>
        public override IEnumerable<bool> CompressFieldsRanges() => Enumerable.Empty<bool>();

        /// <summary>
        /// Получить поля штампа
        /// </summary>
        public IReadOnlyList<IStampTextFieldWord> GetFields() =>
            TableStamp?.CellsElementWord?.
            Where(cell => !String.IsNullOrWhiteSpace(cell.TextNoSpaces)).
            Select(cell => new StampTextFieldWord(cell, CheckFieldType.GetStampFieldType(cell, TableStamp))).
            Where(field => field.StampFieldType != StampFieldType.Unknown).
            ToList();

        /// <summary>
        /// Получить поля штампа согласно типу
        /// </summary>
        public IEnumerable<IStampTextFieldWord> GetFieldsByType(StampFieldType stampFieldType) =>
            FieldsStamp.Where(field => field.StampFieldType == stampFieldType);

        /// <summary>
        /// Получить строку, начиная от индекса маркера
        /// </summary>
        public IRowElementWord GetTableRowByIndex(int rowIndex, int columnStartIndex, int indexColumnFirst, int fieldsCount) =>
            Enumerable.Range(indexColumnFirst, fieldsCount).
            Where(indexColumn => TableStamp.RowsElementWord[rowIndex].CellsElement.Count > columnStartIndex + indexColumn).
            Select(indexColumn => TableStamp.RowsElementWord[rowIndex].CellsElement[columnStartIndex + indexColumn]).
            Map(cells => new RowElementWord(cells));
    }
}