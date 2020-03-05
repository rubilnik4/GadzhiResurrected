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
        private readonly Row _rowElement;

        private readonly List<ICellElementWord> _cellsElementWord;
        public RowElementWord(Row rowElement)
        {
            if (rowElement != null)
            {
                _rowElement = rowElement;
                _cellsElementWord = rowElement.Cells.ToIEnumerable().Select(cell => new CellElementWord(cell)).
                                                     Cast<ICellElementWord>().ToList();
            }
            else
            {
                throw new ArgumentNullException(nameof(rowElement));
            }
        }

        /// <summary>
        /// Список ячеек в строке
        /// </summary>
        public IReadOnlyList<ICellElementWord> CellsElementWord => _cellsElementWord;
    }
}
