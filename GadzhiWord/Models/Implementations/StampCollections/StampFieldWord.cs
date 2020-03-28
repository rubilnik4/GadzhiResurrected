using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Implementation.StampCollections;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiWord.Models.Interfaces.StampCollections;
using GadzhiWord.Word.Interfaces.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiWord.Models.Implementations.StampCollections
{
    /// <summary>
    /// Базовая ячейка штампа Word
    /// </summary>
    public class StampFieldWord : StampField, IStampFieldWord
    {
        /// <summary>
        /// Элемент ячейка определяющая поле штампа
        /// </summary>
        public ICellElement CellElementStamp { get; }

        public StampFieldWord(ICellElement cellElementStamp, StampFieldType stampFieldType)
            : base(stampFieldType)
        {
            CellElementStamp = cellElementStamp ??
                         throw new ArgumentNullException(nameof(cellElementStamp));
        }
    }
}
