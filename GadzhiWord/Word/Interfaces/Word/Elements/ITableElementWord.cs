using System.Collections.Generic;

namespace GadzhiWord.Word.Interfaces.Word.Elements
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
