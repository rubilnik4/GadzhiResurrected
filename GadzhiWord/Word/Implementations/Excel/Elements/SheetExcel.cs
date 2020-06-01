using System;
using GadzhiWord.Extensions.Excel;
using GadzhiWord.Word.Implementations.Excel.Helpers;
using GadzhiWord.Word.Interfaces.Excel.Elements;
using Microsoft.Office.Interop.Excel;
namespace GadzhiWord.Word.Implementations.Excel.Elements
{
    /// <summary>
    /// Лист приложения Excel
    /// </summary>
    public class SheetExcel : ISheetExcel
    {
        /// <summary>
        /// Экземпляр листа
        /// </summary>
        private readonly Worksheet _workSheet;

        public SheetExcel(Worksheet workSheet)
        {
            _workSheet = workSheet ?? throw new ArgumentNullException(nameof(workSheet));
        }

        /// <summary>
        /// Изменить ширину колонки
        /// </summary>
        public void ChangeColumnWidth(int columnIndex, float width)
        {
            if (columnIndex < 0) throw new ArgumentOutOfRangeException(nameof(columnIndex));
            var column = _workSheet.Columns.GetColumnByIndex(columnIndex);
            column.ColumnWidth = width;
        }

        /// <summary>
        /// Вставить данные из буфера
        /// </summary>
        public void PasteFromClipBoard() => _workSheet.Paste();

        /// <summary>
        /// Перейти на новую строку после последней ячейки
        /// </summary>
        public void ToEndNewRow()
        {
            _workSheet.Select();
            int lastRow = _workSheet.Cells.SpecialCells(XlCellType.xlCellTypeLastCell).Row;
            int lastRowIfFirst = (lastRow > 1) ? lastRow + 1 : lastRow;
            string selectedCell = ColumnNamesExcel.GetCellNameByIndexes(0, lastRowIfFirst);
            var selectRange = _workSheet.get_Range(selectedCell);
            selectRange.Select();
        }
    }
}