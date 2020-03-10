using ConvertingModels.Models.Interfaces.ApplicationLibrary.Document;
using GadzhiWord.Extensions.Word;
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
        private IEnumerable<Table> GetTablesInFooters() => _document.Sections.ToIEnumerable().
                                                                     SelectMany(section => section.Footers.ToIEnumerable()).
                                                                     SelectMany(footer => footer.Range.Tables.ToIEnumerable());
    }
}
