using System;
using GadzhiWord.Word.Interfaces;
using GadzhiWord.Word.Interfaces.Excel;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Word;

namespace GadzhiWord.Word.Implementations.Excel
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
           
            Column column = _workSheet.Columns.Item[columnIndex + 1];
            column.Width = width;
        }
    }
}