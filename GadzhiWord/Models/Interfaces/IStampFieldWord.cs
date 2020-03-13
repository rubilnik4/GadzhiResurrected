using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiWord.Word.Interfaces.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiWord.Models.Interfaces
{
    /// <summary>
    /// Базовая ячейка штампа Word
    /// </summary>  
    public interface IStampFieldWord: IStampField
    {
        /// <summary>
        /// Элемент ячейка определяющая поле штампа
        /// </summary>
        ICellElement CellElementStamp { get; }
    }
}
