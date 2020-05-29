using GadzhiWord.Word.Interfaces.Elements;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GadzhiWord.Extensions.StringAdditional;

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

        public CellElementWord(Cell cellElement, int rowIndex, int columnIndex)
        {
            _cellElement = cellElement ?? throw new ArgumentNullException(nameof(cellElement));
            RowIndex = (rowIndex >= 0) ? rowIndex : throw new ArgumentOutOfRangeException(nameof(rowIndex));
            ColumnIndex = (columnIndex >= 0) ? columnIndex : throw new ArgumentOutOfRangeException(nameof(columnIndex));
        }

         /// <summary>
        /// Номер строки
        /// </summary>
        public int RowIndex { get; }

        /// <summary>
        /// Номер колонки
        /// </summary>
        public int ColumnIndex { get; }

        /// <summary>
        /// Текст ячейки
        /// </summary>
        private string _text;

        /// <summary>
        /// Текст ячейки
        /// </summary>
        public string Text => _text ??= _cellElement.Range.Text.PrepareCellTextToCompare();

        /// <summary>
        /// Присутствует ли картинка в ячейке
        /// </summary>
        public bool HasPicture => _cellElement.Range.InlineShapes.Count > 0;

        /// <summary>
        /// Вставить картинку
        /// </summary>
        public void InsertPicture(string filePath)
        {
            if (!String.IsNullOrWhiteSpace(filePath))
            {
                _cellElement.Range.InlineShapes.AddPicture(filePath, false, true);
            }
        }

        /// <summary>
        /// Удалить все картинки
        /// </summary>
        public void DeleteAllPictures()
        {
            foreach (InlineShape shape in _cellElement.Range.InlineShapes)
            {
                shape.Delete();
            }
        }
    }
}
