using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Word.Interfaces.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiApplicationCommon.Models.Interfaces.StampCollections
{
    /// <summary>
    /// Базовая ячейка штампа
    /// </summary>  
    public interface IStampField
    {
        /// <summary>
        /// Элемент ячейка определяющая поле штампа
        /// </summary>
        ICellElement CellElementWord { get; }

        /// <summary>
        /// Тип поля в штампе
        /// </summary>
        StampFieldType StampFieldType { get; }

        /// <summary>
        /// Родительский элемент строка
        /// </summary>
        IRowElement RowElementStamp { get; }
    }
}
