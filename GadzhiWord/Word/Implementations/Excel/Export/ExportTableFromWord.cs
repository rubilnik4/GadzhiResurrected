using System.Collections.Generic;
using System.Linq;
using GadzhiApplicationCommon.Extensions.Functional.Result;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiWord.Models.Implementations.Specification;
using GadzhiWord.Word.Interfaces.Excel.Elements;
using GadzhiWord.Word.Interfaces.Word.Elements;
using Microsoft.Office.Interop.Excel;

namespace GadzhiWord.Word.Implementations.Excel.Export
{
    /// <summary>
    /// Экспорт таблицы из Word
    /// </summary>
    public static class ExportTableFromWord
    {
        /// <summary>
        /// Экспортировать таблицу из Word
        /// </summary>
        public static IResultAppValue<string> ExportTable(IBookExcel bookExcel, IEnumerable<ITableElementWord> tablesWord, string filePath) =>
            new ResultAppValue<IBookExcel>(bookExcel).
            ResultVoidOk(book => ExportTableToSheet(book, tablesWord)).
            ResultValueOkBind(book => book.SaveAs(filePath));

        /// <summary>
        /// Предварительная обработка листа
        /// </summary>
        private static void PrepareSheet(ISheetExcel bookExcel) => 
            TableProcessingSpecification.SetColumnsWidth(bookExcel);

        /// <summary>
        /// Экспорт данных в таблицу Excel
        /// </summary>
        private static void ExportTableToSheet(IBookExcel bookExcel, IEnumerable<ITableElementWord> tablesWord) =>
            new ResultAppValue<ISheetExcel>(bookExcel.Sheets[0]).
            ResultVoidOk(PrepareSheet).
            ResultVoidOk(sheetExcel => CopyTableChunk(sheetExcel, tablesWord));

        /// <summary>
        /// Копировать таблицу через буфер
        /// </summary>
        private static void CopyTableChunk(ISheetExcel sheetExcel, IEnumerable<ITableElementWord> tablesWord)
        {
            foreach (var tableWord in tablesWord)
            {
                tableWord.CopyToClipBoard();
                sheetExcel.ToEndNewRow();
                sheetExcel.PasteFromClipBoard();
            }
        }
    }
}