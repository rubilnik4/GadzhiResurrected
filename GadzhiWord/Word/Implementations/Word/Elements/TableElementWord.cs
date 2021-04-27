using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using GadzhiWord.Extensions.Word;
using GadzhiWord.Word.Implementations.Word.Enums;
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
        public TableElementWord(Table tableElement, IOwnerWord ownerWord)
        {
            _tableElement = tableElement ?? throw new ArgumentNullException(nameof(tableElement));
            ApplicationOffice = ownerWord.ApplicationOffice ?? throw new ArgumentNullException(nameof(ownerWord));
            DocumentWord = ownerWord.DocumentWord;
        }

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
        /// Количество строк в начальной таблице Word без обертки
        /// </summary>
        private int? _rowsCountInitial;

        /// <summary>
        /// Количество строк в начальной таблице Word без обертки
        /// </summary>
        public int RowsCountInitial => _rowsCountInitial ??= _tableElement.Rows.Count;

        /// <summary>
        /// Количество колонок в начальной таблице Word без обертки
        /// </summary>
        private int? _columnsCountInitial;

        /// <summary>
        /// Количество колонок в начальной таблице Word без обертки
        /// </summary>
        public int ColumnsCountInitial => _columnsCountInitial ??= _tableElement.Columns.Count;

        /// <summary>
        /// Тип высоты строк
        /// </summary>
        public HeightRowsType HeightRowsType =>
            _tableElement.Rows.HeightRule switch
            {
                WdRowHeightRule.wdRowHeightAtLeast => HeightRowsType.AtLeast,
                WdRowHeightRule.wdRowHeightExactly => HeightRowsType.Exactly,
                _ => HeightRowsType.Auto,
            };

        /// <summary>
        /// Высота строк
        /// </summary>
        public decimal HeightRows =>
            (decimal)_tableElement.Application.PointsToCentimeters(_tableElement.Rows.Height);

        /// <summary>
        /// Проверить существование ячейки 
        /// </summary>
        public bool HasCellElement(int rowIndex, int columnIndex) =>
            RowsElementWord?.Count > rowIndex && RowsElementWord[rowIndex].CellsElement?.Count > columnIndex;

        /// <summary>
        /// Скопировать таблицу в буфер
        /// </summary>
        public void CopyToClipBoard() => _tableElement.Range.Copy();

        /// <summary>
        /// Установить автоподбор ширины таблицы
        /// </summary>
        public void SetAutoFit(bool autoFit) =>
            _tableElement.AllowAutoFit = autoFit;
        

        /// <summary>
        /// Установить тип высоты строк
        /// </summary>
        public void SetHeightRowsType(HeightRowsType heightRowsType) =>
            _tableElement.Rows.HeightRule = heightRowsType switch
            {
                HeightRowsType.AtLeast => WdRowHeightRule.wdRowHeightAtLeast,
                HeightRowsType.Exactly => WdRowHeightRule.wdRowHeightExactly,
                _ => WdRowHeightRule.wdRowHeightAuto
            };

        /// <summary>
        /// Установить высоту строк
        /// </summary>
        public void SetHeightRows(decimal height) =>
            _tableElement.Rows.Height = _tableElement.Application.CentimetersToPoints((float)height);

        /// <summary>
        /// Получить строки таблицы
        /// </summary>       
        private IReadOnlyList<IRowElementWord> GetRowsElement() =>
            _tableElement.Range.Cells.ToEnumerable().
            Select(cell => cell.RowIndex).
            Distinct().
            OrderBy(indexRowOriginal => indexRowOriginal).
            Select((indexRowOriginal, indexRowNew) => new RowElementWord(GetCellsElementByRow(indexRowOriginal, indexRowNew))).
            ToList();

        /// <summary>
        /// Получить ячейки таблицы
        /// </summary>
        private IEnumerable<ICellElementWord> GetCellsElementByRow(int indexRowOriginal, int indexRowNew) =>
            _tableElement.Range.Cells.ToEnumerable().
            Where(cell => cell.RowIndex == indexRowOriginal).
            Select((cell, indexColumnNew) => new CellElementWord(cell, indexRowNew, indexColumnNew));
    }
}
