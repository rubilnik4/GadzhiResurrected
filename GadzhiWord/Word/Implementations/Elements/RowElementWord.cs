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

        public RowElementWord(List<ICellElement> cellsElementWord)
        {
            _cellsElementWord = cellsElementWord ?? throw new ArgumentNullException(nameof(cellsElementWord));
        }

        /// <summary>
        /// Список ячеек в строке
        /// </summary>
        public IList<ICellElement> CellsElement => _cellsElementWord;

        /// <summary>
        /// Индекс строки
        /// </summary>
        public int Index => _cellsElementWord[0].RowIndex;
    }
}
