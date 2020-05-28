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
        private IReadOnlyList<IStampFieldWord> _fieldsStamp;

        /// <summary>
        /// Получить поля штампа
        /// </summary>
        protected IReadOnlyList<IStampFieldWord> FieldsStamp => _fieldsStamp ??= GetFields();

        /// <summary>
        /// Вписать текстовые поля в рамки
        /// </summary>
        public override IEnumerable<bool> CompressFieldsRanges() => Enumerable.Empty<bool>();

        /// <summary>
        /// Получить поля штампа
        /// </summary>
        private IReadOnlyList<IStampFieldWord> GetFields() =>
            TableStamp?.CellsElementWord?.
            Where(cell => !String.IsNullOrWhiteSpace(cell.Text)).
            Select(cell => new StampFieldWord(cell, CheckFieldType.GetStampFieldType(cell, TableStamp))).
            Where(field => field.StampFieldType != StampFieldType.Unknown).
            ToList();
    }
}