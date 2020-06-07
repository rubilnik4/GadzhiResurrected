using System;
using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiApplicationCommon.Models.Implementation.StampCollections.Fields;
using GadzhiWord.Models.Interfaces.StampCollections;
using GadzhiWord.Word.Interfaces.Word.Elements;

namespace GadzhiWord.Models.Implementations.StampCollections.Fields
{
    /// <summary>
    /// Базовая ячейка штампа Word
    /// </summary>
    public class StampFieldWord : StampField, IStampFieldWord
    {
        public StampFieldWord(ICellElementWord cellElementStamp, StampFieldType stampFieldType)
            : base(stampFieldType)
        {
            CellElementStamp = cellElementStamp ?? throw new ArgumentNullException(nameof(cellElementStamp));
        }

        /// <summary>
        /// Элемент ячейка определяющая поле штампа
        /// </summary>
        public ICellElementWord CellElementStamp { get; }

        /// <summary>
        /// Вертикальное расположение поля
        /// </summary>
        public bool IsVertical => StampFieldType == StampFieldType.ApprovalChangeSignature || 
                                  StampFieldType == StampFieldType.ApprovalSignature;
    }
}
