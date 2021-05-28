using System.Collections.Generic;
using System.Windows.Forms;
using GadzhiApplicationCommon.Extensions.Functional.Result;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiWord.Models.Enums;
using GadzhiWord.Models.Implementations.Specification.Table;
using GadzhiWord.Models.Interfaces.Specification;
using GadzhiWord.Word.Interfaces.Excel.Elements;
using GadzhiWord.Word.Interfaces.Word.Elements;

namespace GadzhiWord.Infrastructure.Implementations.Specification
{
    /// <summary>
    /// Экспорт таблицы из Word
    /// </summary>
    public static class SpecificationToExcel
    {
        /// <summary>
        /// Экспортировать спецификацию из Word
        /// </summary>
        public static IResultAppValue<string> Export(IBookExcel bookExcel, ISpecification specification, string filePath) =>
            new ResultAppValue<IBookExcel>(bookExcel).
            ResultVoidOk(book => ExportTableToSheet(book, specification)).
            ResultValueOkBind(book => book.SaveAs(filePath));

        /// <summary>
        /// Экспорт спецификации в таблицу Excel
        /// </summary>
        private static void ExportTableToSheet(IBookExcel bookExcel, ISpecification specification) =>
            new ResultAppValue<ISheetExcel>(bookExcel.Sheets[0]).
            ResultVoidOk(sheetExcel => PrepareSheet(sheetExcel, specification.GetSpecificationType())).
            ResultVoidOk(sheetExcel => CopyTableChunk(sheetExcel, specification.TablesWord));

        /// <summary>
        /// Предварительная обработка листа
        /// </summary>
        private static void PrepareSheet(ISheetExcel sheetExcel, SpecificationType specificationType) =>
            TableProcessingSpecification.SetColumnsWidth(sheetExcel, specificationType);


        /// <summary>
        /// Копировать таблицу через буфер
        /// </summary>
        private static void CopyTableChunk(ISheetExcel sheetExcel, IEnumerable<ITableElementWord> tablesElement)
        {
            foreach (var tableElement in tablesElement)
            {
                tableElement.CopyToClipBoard();
                sheetExcel.ToEndNewRow();
                sheetExcel.PasteFromClipBoard();
            }
        }
    }
}