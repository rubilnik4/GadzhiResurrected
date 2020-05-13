using GadzhiApplicationCommon.Models.Interfaces.ApplicationLibrary.Document;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiWord.Extensions.Word;
using GadzhiWord.Models.Implementations.StampCollections;
using GadzhiWord.Word.Implementations.Elements;
using GadzhiWord.Word.Interfaces.Elements;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Implementation.StampCollections;
using GadzhiWord.Extensions.StringAdditional;

namespace GadzhiWord.Word.Implementations.DocumentWordPartial
{
    /// <summary>
    /// Подкласс документа Word для работы с элементами
    /// </summary>
    public partial class DocumentWord : IDocumentLibraryElements
    {
        /// <summary>
        /// Найти все штампы во всех моделях и листах
        /// </summary>       
        public IStampContainer StampContainer { get; }

        /// <summary>
        /// Найти таблицы в колонтитулах
        /// </summary>
        private IEnumerable<IStamp> FindStamps() => _document.Sections.ToIEnumerable().
                                                    SelectMany(section => section.Footers.ToIEnumerable()).
                                                    SelectMany(footer => footer.Range.Tables.ToIEnumerable()).
                                                    Select(table => new TableElementWord(table, ToOwnerWord())).
                                                    Where(CheckFooterIsStamp).
                                                    Select((tableElement, stampIndex) => new StampMainWord(tableElement, 
                                                                                                           new StampIdentifier(stampIndex),
                                                                                                           PaperSize, OrientationType));

        /// <summary>
        /// Проверить является ли колонтитул штампом
        /// </summary>
        private static bool CheckFooterIsStamp(ITableElement tableElement) => tableElement.CellsElementWord.
                                                                       Where(cell => !String.IsNullOrWhiteSpace(cell?.Text)).
                                                                       Select(cell => cell.Text.PrepareCellTextToCompare()).
                                                                       Any(cellText => StampSettingsWord.MarkersMainStamp.MarkerContain(cellText));

        /// <summary>
        /// Преобразовать к виду родительского элемента
        /// </summary>      
        public IOwnerWord ToOwnerWord() => new OwnerWord(this);
    }
}
