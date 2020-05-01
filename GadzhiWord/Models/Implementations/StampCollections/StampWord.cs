using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GadzhiApplicationCommon.Functional;
using GadzhiApplicationCommon.Models.Enums;
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
        /// <summary>
        /// Элемент таблица
        /// </summary>
        protected ITableElement TableStamp { get; }

        protected StampWord(ITableElement tableStamp, string paperSize, OrientationType orientationType)
        {
            TableStamp = tableStamp;
            PaperSize = paperSize;
            Orientation = orientationType;
        }
        /// <summary>
        /// Наименование
        /// </summary>
        public override string Name => $"{StampSettingsWord.StampTypeToString[StampType]}";

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
        protected IEnumerable<IStampFieldWord> FieldsStamp => GetFields();

        /// <summary>
        /// Получить поля штампа
        /// </summary>
        private IEnumerable<IStampFieldWord> GetFields() =>
            TableStamp?.CellsElementWord?.
                        Where(cell => !String.IsNullOrWhiteSpace(cell.Text)).
                        Select(cell => new StampFieldWord(cell, CheckFieldType.GetStampFieldType(cell, TableStamp))).
                        Where(field => field.StampFieldType != StampFieldType.Unknown);

        /// <summary>
        /// Вписать текстовые поля в рамки
        /// </summary>
        public override IEnumerable<bool> CompressFieldsRanges() => Enumerable.Empty<bool>();
    }
}
