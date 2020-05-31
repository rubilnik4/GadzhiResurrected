using GadzhiApplicationCommon.Extensions.Functional.Result;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiWord.Word.Interfaces.Excel.Elements;

namespace GadzhiWord.Word.Implementations.Excel
{
    /// <summary>
    /// Экспорт таблицы из Word
    /// </summary>
    public static class ExportTableFromWord
    {
        /// <summary>
        /// Экспортировать таблицу из Word
        /// </summary>
        public static IResultAppValue<string> ExportTable(IBookExcel bookExcel, string filePath) =>
            new ResultAppValue<IBookExcel>(bookExcel).
            ResultVoidOk(ExportDataToSheet).
            ResultValueOkBind(book => book.SaveAs(filePath));

        /// <summary>
        /// Экспорт данных в таблицу Excel
        /// </summary>
        private static void ExportDataToSheet(IBookExcel bookExcel) =>
            new ResultAppValue<ISheetExcel>(bookExcel.Sheets[0]).
            ResultVoidOk(SetColumnsWidth);

        /// <summary>
        /// Установить ширину колонок
        /// </summary>
        private static void SetColumnsWidth(ISheetExcel sheetExcel)
        {
            sheetExcel.ChangeColumnWidth(0, 7.65f);
            sheetExcel.ChangeColumnWidth(1, 54f);
            sheetExcel.ChangeColumnWidth(2, 25f);
            sheetExcel.ChangeColumnWidth(3, 18f);
            sheetExcel.ChangeColumnWidth(4, 7.56f);
            sheetExcel.ChangeColumnWidth(5, 7.56f);
            sheetExcel.ChangeColumnWidth(6, 9.67f);
            sheetExcel.ChangeColumnWidth(7, 16.11f);
        }
    }
}