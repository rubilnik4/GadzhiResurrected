using System;
using System.Collections.Generic;
using System.Linq;
using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiWord.Models.Implementations.StampCollections.Fields;
using GadzhiWord.Models.Interfaces.StampCollections;

namespace GadzhiWord.Models.Implementations.StampCollections.StampPartial
{
    /// <summary>
    /// Подкласс для работа с внутренними полями штампа
    /// </summary>
    public partial class StampWord
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
        private IReadOnlyList<IStampTextFieldWord> GetFields() =>
            TableStamp?.CellsElementWord?.
            Where(cell => !String.IsNullOrWhiteSpace(cell.Text)).
            Select(cell => new StampTextFieldWord(cell, CheckFieldType.GetStampFieldType(cell, TableStamp))).
            Where(field => field.StampFieldType != StampFieldType.Unknown).
            ToList();

        /// <summary>
        /// Получить поля штампа согласно типу
        /// </summary>
        protected IEnumerable<IStampTextFieldWord> GetFieldsByType(StampFieldType stampFieldType) =>
            FieldsStamp.Where(field => field.StampFieldType == stampFieldType);
    }
}