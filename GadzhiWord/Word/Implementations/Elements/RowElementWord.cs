using GadzhiWord.Extensions.Word;
using GadzhiWord.Word.Interfaces.Elements;
using Microsoft.Office.Interop.Word;
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
    public class RowElementWord : IRowElementWord
    {
        /// <summary>
        /// Элемент строка Word
        /// </summary>
        private readonly List<ICellElementWord> _cellsElementWord;

        /// <summary>
        /// Родительская таблица
        /// </summary>
        private readonly ITableElementWord _tableElementWord;

        public RowElementWord(List<ICellElementWord> cellsElementWord, ITableElementWord tableElementWord)
        {
            if (cellsElementWord != null)
            {
                _cellsElementWord = cellsElementWord;
                _tableElementWord = tableElementWord;
            }
            else
            {
                throw new ArgumentNullException(nameof(cellsElementWord));
            }
        }

        /// <summary>
        /// Список ячеек в строке
        /// </summary>
        public IReadOnlyList<ICellElementWord> CellsElementWord => _cellsElementWord;
    }
}
