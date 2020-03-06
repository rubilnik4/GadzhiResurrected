using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiWord.Word.Interfaces.Elements
{
    /// <summary>
    /// Элемент таблица
    /// </summary>
    public interface ITableElementWord
    {
        /// <summary>
        /// Получить ячейки таблицы
        /// </summary>
        IEnumerable<ICellElementWord> CellsElementWord { get; }

        /// <summary>
        /// Получить строки таблицы
        /// </summary>
        IReadOnlyList<IRowElementWord> RowsElementWord { get; }
    }
}
