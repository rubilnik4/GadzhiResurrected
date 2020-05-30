using System;
using System.Collections.Generic;
using System.Linq;
using GadzhiWord.Extensions.Excel;
using GadzhiWord.Word.Interfaces.Excel;
using Microsoft.Office.Interop.Excel;

namespace GadzhiWord.Word.Implementations.Excel
{
    public class BookExcel: IBookExcel
    {
        /// <summary>
        /// Экземпляр листа
        /// </summary>
        private readonly Workbook _workbook;

        public BookExcel(Workbook workbook)
        {
            _workbook = workbook ?? throw new ArgumentNullException(nameof(workbook));
        }

        /// <summary>
        /// Листы в книге
        /// </summary>
        private IReadOnlyList<ISheetExcel> _sheets;

        /// <summary>
        /// Листы в книге
        /// </summary>
        public IReadOnlyList<ISheetExcel> Sheets => _sheets ??= GetSheetsExcel();

        /// <summary>
        /// Получить список листов
        /// </summary>
        private IReadOnlyList<ISheetExcel> GetSheetsExcel() =>
            _workbook.Sheets.ToIEnumerable().
            Select(sheet => new SheetExcel(sheet)).
            ToList();
    }
}