using System.Collections.Generic;
using System.Linq;
using GadzhiApplicationCommon.Extensions.Functional;
using GadzhiApplicationCommon.Extensions.Functional.Result;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiWord.Infractructure.Implementations.Specification;
using GadzhiWord.Models.Implementations.Specification;
using GadzhiWord.Models.Interfaces.Specification;
using GadzhiWord.Word.Interfaces.Excel.Elements;
using GadzhiWord.Word.Interfaces.Word.Elements;

namespace GadzhiWord.Word.Implementations.Word.DocumentWordPartial
{
    /// <summary>
    /// Подкласс документа Word для экспорта
    /// </summary>
    public partial class DocumentWord
    {
        /// <summary>
        /// Отфильтровать таблицы Word для экспорта
        /// </summary>
        private static IEnumerable<ITableElementWord> FilteringExportTables(IEnumerable<ITableElementWord> tablesWord) =>
            tablesWord.Where(Specification.IsTableSpecification);

        /// <summary>
        /// Экспортировать таблицы Word по их типу
        /// </summary>
        private IResultAppValue<string> ExportByTableType(IEnumerable<ITableElementWord> tablesWord, string filePath, StampDocumentType stampDocumentType) =>
            stampDocumentType switch
            {
                StampDocumentType.Specification => ExportSpecificationToExcel(new Specification(tablesWord), filePath),
                _ => new ResultAppValue<string>(new ErrorApplication(ErrorApplicationType.TableNotFound, "Тип таблиц не определен")),
            };

        /// <summary>
        /// Экспортировать данные таблицы Word в Excel
        /// </summary>
        private IResultAppValue<string> ExportSpecificationToExcel(ISpecification specification, string filePath) =>
            new ResultAppValue<IBookExcel>(ApplicationOffice.CreateWorkbook()).
                ResultValueOkBind(book => SpecificationToExcel.Export(book, specification, filePath).
                                                               Void(_ => book.Close()));
    }
}