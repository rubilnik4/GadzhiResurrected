using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiWord.Word.Interfaces.Elements
{
    /// <summary>
    /// Элемент таблица. Базовый вариант
    /// </summary>
    public interface ITableElement
    {
        /// <summary>
        /// Получить ячейки таблицы
        /// </summary>
        IEnumerable<ICellElement> CellsElementWord { get; }

        /// <summary>
        /// Получить строки таблицы
        /// </summary>
        IList<IRowElement> RowsElementWord { get; }
    }
}
