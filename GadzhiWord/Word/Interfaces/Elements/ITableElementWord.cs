using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiWord.Word.Interfaces.Elements
{
    /// <summary>
    /// Элемент таблица. Базовый вариант
    /// </summary>
    public interface ITableElementWord: IOwnerWord
    {
        /// <summary>
        /// Получить строки таблицы
        /// </summary>
        IReadOnlyList<IRowElementWord> RowsElementWord { get; }

        /// <summary>
        /// Получить ячейки таблицы
        /// </summary>
        IEnumerable<ICellElementWord> CellsElementWord { get; }

        /// <summary>
        /// Проверить существование ячейки 
        /// </summary>
        bool HasCellElement(int rowIndex, int columnIndex);
    }
}
