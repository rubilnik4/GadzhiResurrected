using System;
using GadzhiApplicationCommon.Extensions.StringAdditional;
using GadzhiApplicationCommon.Helpers;
using GadzhiWord.Models.Implementations.StampCollections;
using GadzhiWord.Word.Interfaces.Word.Elements;
using Microsoft.Office.Interop.Word;

namespace GadzhiWord.Word.Implementations.Word.Elements
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
        public string Text => _text ??= _cellElement.Range.Text.RemoveArtefacts();

        /// <summary>
        /// Текст ячейки без пробелов
        /// </summary>
        public string TextNoSpaces => _text ??= _cellElement.Range.Text.RemoveSpacesAndArtefacts();

        /// <summary>
        /// Получить слово максимальной длины
        /// </summary>
        public string MaxLengthWord => TextFormatting.GetMaxLengthWord(Text);

        /// <summary>
        /// Присутствует ли картинка в ячейке
        /// </summary>
        public bool HasPicture => _cellElement.Range.InlineShapes.Count > 0;

        /// <summary>
        /// Вставить картинку
        /// </summary>
        public void InsertPicture(string filePath)
        {
            if (String.IsNullOrWhiteSpace(filePath)) return;
            if (_cellElement.Range.InlineShapes.Count > 0) _cellElement.Range.Delete();
            if (_cellElement.HeightRule != WdRowHeightRule.wdRowHeightExactly) _cellElement.HeightRule = WdRowHeightRule.wdRowHeightExactly;
            if (_cellElement.Height == 0) _cellElement.Height = _cellElement.Application.CentimetersToPoints((float)StampSettingsWord.HEIGHT_TABLE_ROW);

            _cellElement.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            _cellElement.VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;
            _cellElement.Range.InlineShapes.AddPicture(filePath, false, true);
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
