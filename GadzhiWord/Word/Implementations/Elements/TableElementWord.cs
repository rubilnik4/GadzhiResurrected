using GadzhiApplicationCommon.Word.Interfaces.Elements;
using GadzhiWord.Extensions.Word;
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
    public class TableElementWord : ITableElement
    {
        /// <summary>
        /// Элемент таблица Word
        /// </summary>
        private readonly Table _tableElement;

        public TableElementWord(Table tableElement)
        {
            _tableElement = tableElement;
            RowsElement.Count();
        }

        /// <summary>
        /// Получить ячейки таблицы
        /// </summary>
        public IEnumerable<ICellElement> CellsElement => _tableElement?.Range.Cells.ToIEnumerable().
                                                                  Select(cell => new CellElementWord(cell, this));

        /// <summary>
        /// Получить строки таблицы
        /// </summary>
        public IList<IRowElement> RowsElement => GetRowsElement();

        /// <summary>
        /// Получить строки таблицы
        /// </summary>       
        private List<IRowElement> GetRowsElement()
        {
            var number = _tableElement.Rows.Count - 1;
            var rowsElementWord = Enumerable.Range(0, _tableElement.Rows.Count).
                                             Select(index => new List<ICellElement>()).ToList();

            foreach (var cell in CellsElement)
            {
                var row = rowsElementWord[cell.RowIndex];                
                row.Add(cell);
            }
            return rowsElementWord?.Select(row => new RowElementWord(row, this)).Cast<IRowElement>().ToList();
        }
    }
}
