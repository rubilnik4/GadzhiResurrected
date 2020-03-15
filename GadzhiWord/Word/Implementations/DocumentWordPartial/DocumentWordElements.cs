using GadzhiApplicationCommon.Models.Interfaces.ApplicationLibrary.Document;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiWord.Extension.StringAdditional;
using GadzhiWord.Extensions.Word;
using GadzhiWord.Models.Implementations.StampCollections;
using GadzhiWord.Word.Implementations.Elements;
using GadzhiWord.Word.Interfaces.Elements;
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
        /// Найти таблицы в колонтитулах
        /// </summary>
        public IEnumerable<IStamp> FindStamps() => _document.Sections.ToIEnumerable().
                                                                      SelectMany(section => section.Footers.ToIEnumerable()).
                                                                      SelectMany(footer => footer.Range.Tables.ToIEnumerable()).
                                                                      Select(table => new TableElementWord(table)).
                                                                      Where(tableElement => CheckFooterIsStamp(tableElement)).
                                                                      Select(tableElement => new StampMainWord(tableElement, PaperSize, OrientationType));

        /// <summary>
        /// Проверить является ли колонтитул штампом
        /// </summary>
        private bool CheckFooterIsStamp(ITableElement tableElement) => tableElement.CellsElementWord.
                                                                       Where(cell => !String.IsNullOrWhiteSpace(cell?.Text)).
                                                                       Select(cell => StringAdditionalExtensions.PrepareCellTextToCompare(cell?.Text)).
                                                                       Any(cellText => StampSettingsWord.MarkersMainStamp.MarkerContain(cellText));
    }
}
