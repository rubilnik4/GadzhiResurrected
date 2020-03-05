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
    /// Элемент ячейка
    /// </summary>
    public class CellElementWord : ICellElementWord
    {
        /// <summary>
        /// Элемент ячейка Word
        /// </summary>
        private readonly Cell _cellElement;

        public CellElementWord(Cell cellElement)
        {
            if (cellElement != null)
            {
                _cellElement = cellElement;
            }
            else
            {
                throw new ArgumentNullException(nameof(cellElement));
            }
        }

        /// <summary>
        /// Текст ячейки
        /// </summary>
        public string Text => _cellElement.Range.Text;
    }
}
