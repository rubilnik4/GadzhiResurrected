using System;
using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiApplicationCommon.Models.Implementation.StampCollections.Fields;
using GadzhiWord.Models.Interfaces.StampCollections;
using GadzhiWord.Word.Interfaces.Elements;

namespace GadzhiWord.Models.Implementations.StampCollections.Fields
{
    /// <summary>
    /// Базовая ячейка штампа Word
    /// </summary>
    public class StampFieldWord : StampField, IStampFieldWord
    {
        public StampFieldWord(ICellElement cellElementStamp, StampFieldType stampFieldType)
            : base(stampFieldType)
        {
            CellElementStamp = cellElementStamp ?? throw new ArgumentNullException(nameof(cellElementStamp));
        }

        /// <summary>
        /// Элемент ячейка определяющая поле штампа
        /// </summary>
        public ICellElement CellElementStamp { get; }
    }
}
