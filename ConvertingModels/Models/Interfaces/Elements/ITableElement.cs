using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiConverting.Word.Interfaces.Elements
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
        IReadOnlyList<IRowElement> RowsElementWord { get; }
    }
}
