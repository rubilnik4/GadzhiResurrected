using GadzhiWord.Extensions.Word;
using GadzhiWord.Word.Interfaces;
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
    public class TableElementWord : ITableElement
    {
        /// <summary>
        /// Элемент таблица Word
        /// </summary>
        private readonly Table _tableElement;

        /// <summary>
        /// Класс для работы с приложением Microstation
        /// </summary>
        public IApplicationWord ApplicationWord { get; }

        /// <summary>
        /// Модель или лист в файле
        /// </summary>
        public IDocumentWord DocumentWord { get; }

        public TableElementWord(Table tableElement, IOwnerWord ownerWord)
        {
            _tableElement = tableElement ?? throw new ArgumentNullException(nameof(tableElement));
            ApplicationWord = ownerWord.ApplicationWord ?? throw new ArgumentNullException(nameof(ownerWord));
            DocumentWord = ownerWord.DocumentWord ;
        }

        /// <summary>
        /// Получить ячейки таблицы
        /// </summary>
        public IEnumerable<ICellElement> CellsElementWord => _tableElement?.Range.Cells.ToIEnumerable().
                                                              Select(cell => new CellElementWord(cell, this));

        /// <summary>
        /// Получить строки таблицы
        /// </summary>
        public IList<IRowElement> RowsElementWord => GetRowsElement();


        /// <summary>
        /// Проверить существование ячейки 
        /// </summary>
        public bool HasCellElement(int rowIndex, int columnIndex) =>
            RowsElementWord?.Count >= rowIndex && RowsElementWord[rowIndex].CellsElementWord?.Count >= columnIndex;


        /// <summary>
        /// Получить строки таблицы
        /// </summary>       
        private List<IRowElement> GetRowsElement()
        {
            var rowsElementWord = Enumerable.Range(0, _tableElement.Rows.Count).
                                  Select(index => new List<ICellElement>()).ToList();

            foreach (var cell in CellsElementWord)
            {
                var row = rowsElementWord[cell.RowIndex];
                row.Add(cell);
            }
            return rowsElementWord.Select(row => new RowElementWord(row)).Cast<IRowElement>().ToList();
        }
    }
}
