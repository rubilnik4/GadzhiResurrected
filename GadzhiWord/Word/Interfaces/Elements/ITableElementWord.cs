using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiWord.Word.Interfaces.Elements
{
    /// <summary>
    /// Элемент таблица. Базовый вариант
    /// </summary>
    public interface ITableElement: IOwnerWord
    {
        /// <summary>
        /// Получить ячейки таблицы
        /// </summary>
        IReadOnlyList<ICellElement> CellsElementWord { get; }

        /// <summary>
        /// Получить строки таблицы
        /// </summary>
        IReadOnlyList<IRowElement> RowsElementWord { get; }

        /// <summary>
        /// Проверить существование ячейки 
        /// </summary>
        bool HasCellElement(int rowIndex, int columnIndex);
    }
}
