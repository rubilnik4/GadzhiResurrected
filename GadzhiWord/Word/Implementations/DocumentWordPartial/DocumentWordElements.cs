using GadzhiApplicationCommon.Models.Interfaces.ApplicationLibrary.Document;
using GadzhiApplicationCommon.Word.Interfaces.Elements;
using GadzhiWord.Extensions.Word;
using GadzhiWord.Word.Implementations.Elements;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiWord.Word.Implementations.DocumentWordPartial
{
    /// <summary>
    /// Подкласс документа Word для работы с элементами
    /// </summary>
    public partial class DocumentWord : IDocumentLibraryElements
    {
        /// <summary>
        /// Найти нижние колонтитулы
        /// </summary>
        public IEnumerable<ITableElement> GetTablesInFooters() => _document.Sections.ToIEnumerable().
                                                                   SelectMany(section => section.Footers.ToIEnumerable()).
                                                                   SelectMany(footer => footer.Range.Tables.ToIEnumerable()).
                                                                   Select(table => new TableElementWord(table));
    }
}
