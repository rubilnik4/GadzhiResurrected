using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiApplicationCommon.Word.Interfaces.Elements
{
    /// <summary>
    /// Элемент таблица. Базовый вариант
    /// </summary>
    public interface ITableElement
    {
        /// <summary>
        /// Получить ячейки таблицы
        /// </summary>
        IEnumerable<ICellElement> CellsElement { get; }

        /// <summary>
        /// Получить строки таблицы
        /// </summary>
        IList<IRowElement> RowsElement { get; }
    }
}
