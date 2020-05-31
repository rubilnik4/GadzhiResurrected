﻿using System;
using System.Collections.Generic;
using System.Linq;
using GadzhiWord.Extensions.Word;
using GadzhiWord.Word.Interfaces;
using GadzhiWord.Word.Interfaces.Word;
using GadzhiWord.Word.Interfaces.Word.Elements;
using Microsoft.Office.Interop.Word;

namespace GadzhiWord.Word.Implementations.Word.Elements
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

        /// <summary>
        /// Класс для работы с приложением Microstation
        /// </summary>
        public IApplicationOffice ApplicationOffice { get; }

        /// <summary>
        /// Модель или лист в файле
        /// </summary>
        public IDocumentWord DocumentWord { get; }

        public TableElementWord(Table tableElement, IOwnerWord ownerWord)
        {
            _tableElement = tableElement ?? throw new ArgumentNullException(nameof(tableElement));
            ApplicationOffice = ownerWord.ApplicationOffice ?? throw new ArgumentNullException(nameof(ownerWord));
            DocumentWord = ownerWord.DocumentWord;
        }

        /// <summary>
        /// Получить строки таблицы
        /// </summary>
        private IReadOnlyList<IRowElementWord> _rowsElementWord;

        /// <summary>
        /// Получить строки таблицы
        /// </summary>
        public IReadOnlyList<IRowElementWord> RowsElementWord => _rowsElementWord ??= GetRowsElement();

        /// <summary>
        /// Получить ячейки таблицы
        /// </summary>
        public IEnumerable<ICellElementWord> CellsElementWord => RowsElementWord.SelectMany(row => row.CellsElement);

        /// <summary>
        /// Проверить существование ячейки 
        /// </summary>
        public bool HasCellElement(int rowIndex, int columnIndex) =>
            RowsElementWord?.Count >= rowIndex && RowsElementWord[rowIndex].CellsElement?.Count >= columnIndex;

        /// <summary>
        /// Получить строки таблицы
        /// </summary>       
        private IReadOnlyList<IRowElementWord> GetRowsElement() =>
            _tableElement.Range.Cells.ToIEnumerable().
            Select(cell => cell.RowIndex).
            Distinct().
            OrderBy(indexRowOriginal => indexRowOriginal).
            Select((indexRowOriginal, indexRowNew) => new RowElementWord(GetCellsElementByRow(indexRowOriginal, indexRowNew))).
            ToList();

        /// <summary>
        /// Получить ячейки таблицы
        /// </summary>
        private IEnumerable<ICellElementWord> GetCellsElementByRow(int indexRowOriginal, int indexRowNew) =>
            _tableElement.Range.Cells.ToIEnumerable().
            Where(cell => cell.RowIndex == indexRowOriginal).
            Select((cell, indexColumnNew) => new CellElementWord(cell, indexRowNew, indexColumnNew));
    }
}