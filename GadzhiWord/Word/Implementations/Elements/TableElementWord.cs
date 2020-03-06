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
    /// Элемент таблица
    /// </summary>
    public class TableElementWord : ITableElementWord
    {
        /// <summary>
        /// Элемент таблица Word
        /// </summary>
        private readonly Table _tableElement;

        public TableElementWord(Table tableElement)
        {
            _tableElement = tableElement;
            RowsElementWord.Count();
        }

        /// <summary>
        /// Получить ячейки таблицы
        /// </summary>
        public IEnumerable<ICellElementWord> CellsElementWord => _tableElement?.Range.Cells.ToIEnumerable().
                                                                  Select(cell => new CellElementWord(cell, this));

        /// <summary>
        /// Получить строки таблицы
        /// </summary>
        public IReadOnlyList<IRowElementWord> RowsElementWord => GetRowsElementWord();

        /// <summary>
        /// Получить строки таблицы
        /// </summary>       
        private List<IRowElementWord> GetRowsElementWord()
        {
            var number = _tableElement.Rows.Count - 1;
            var rowsElementWord = Enumerable.Range(0, _tableElement.Rows.Count).
                                             Select(index => new List<ICellElementWord>()).ToList();

            foreach (var cell in CellsElementWord)
            {
                var row = rowsElementWord[cell.RowIndex];                
                row.Add(cell);
            }
            return rowsElementWord?.Select(row => new RowElementWord(row, this)).Cast<IRowElementWord>().ToList();
        }
    }
}
