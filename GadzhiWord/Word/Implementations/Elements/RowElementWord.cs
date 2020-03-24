using GadzhiWord.Word.Interfaces.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiWord.Word.Implementations.Elements
{
    /// <summary>
    /// Элемент строка
    /// </summary>
    public class RowElementWord : IRowElement
    {
        /// <summary>
        /// Элемент строка Word
        /// </summary>
        private readonly List<ICellElement> _cellsElementWord;

        /// <summary>
        /// Родительская таблица
        /// </summary>
        private readonly ITableElement _tableElementWord;

        public RowElementWord(List<ICellElement> cellsElementWord, ITableElement tableElementWord)
        {
            _cellsElementWord = cellsElementWord ?? throw new ArgumentNullException(nameof(cellsElementWord));
            _tableElementWord = tableElementWord ?? throw new ArgumentNullException(nameof(tableElementWord));
        }

        /// <summary>
        /// Список ячеек в строке
        /// </summary>
        public IList<ICellElement> CellsElementWord => _cellsElementWord;

        /// <summary>
        /// Индекс строки
        /// </summary>
        public int Index => _cellsElementWord[0].RowIndex;
    }
}
