using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiApplicationCommon.Word.Interfaces.Elements
{
    /// <summary>
    /// Элемент строка. Базовый вариант
    /// </summary>
    public interface IRowElement
    {
        /// <summary>
        /// Список ячеек в строке
        /// </summary>
        IList<ICellElement> CellsElement { get; }
    }
}
