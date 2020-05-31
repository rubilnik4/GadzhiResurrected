using System;
using GadzhiWord.Extensions.Excel;
using GadzhiWord.Word.Interfaces.Excel;
using GadzhiWord.Word.Interfaces.Excel.Elements;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Word;

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
            var column = _workSheet.Columns.GetColumnByIndex(columnIndex );
            column.ColumnWidth = width;
        }
    }
}