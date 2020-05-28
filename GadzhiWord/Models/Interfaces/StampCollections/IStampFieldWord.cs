
using GadzhiWord.Word.Interfaces.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Fields;

namespace GadzhiWord.Models.Interfaces.StampCollections
{
    /// <summary>
    /// Базовая ячейка штампа Word
    /// </summary>  
    public interface IStampFieldWord : IStampField
    {
        /// <summary>
        /// Элемент ячейка, определяющая поле штампа
        /// </summary>
        ICellElement CellElementStamp { get; }
    }
}
