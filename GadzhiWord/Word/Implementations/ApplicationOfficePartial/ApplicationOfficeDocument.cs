using GadzhiApplicationCommon.Extensions.Functional;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiWord.Word.Implementations.Excel;
using GadzhiWord.Word.Implementations.Excel.Elements;
using GadzhiWord.Word.Interfaces.Excel;
using GadzhiWord.Word.Interfaces.Excel.Elements;
using GadzhiWord.Word.Interfaces.Word;
using Microsoft.Office.Interop.Excel;
using DocumentWord = GadzhiWord.Word.Implementations.Word.DocumentWordPartial.DocumentWord;

namespace GadzhiWord.Word.Implementations.ApplicationOfficePartial
{
    /// <summary>
    /// Подкласс приложения Word для работы с документом
    /// </summary>
    public partial class ApplicationOffice
    {
        /// <summary>
        /// Открыть документ
        /// </summary>
        public IResultAppValue<IDocumentWord> OpenDocument(string filePath) =>
           new ResultAppValue<IDocumentWord>(new DocumentWord(ApplicationWord.Documents.Open(filePath), this),
                                             new ErrorApplication(ErrorApplicationType.FileNotOpen, "Документ Word не создан"));

        /// <summary>
        /// Создать новую книгу Excel
        /// </summary>
        public IBookExcel CreateWorkbook() =>
            ApplicationExcel.Workbooks.Add(XlWBATemplate.xlWBATWorksheet).
            Map(workBook => new BookExcel(workBook));
    }
}
