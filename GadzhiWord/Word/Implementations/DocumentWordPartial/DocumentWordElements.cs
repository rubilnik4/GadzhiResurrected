using GadzhiApplicationCommon.Models.Interfaces.ApplicationLibrary.Document;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiMicrostation.Microstation.Implementations.Elements;
using GadzhiWord.Extension.StringAdditional;
using GadzhiWord.Extensions.Word;
using GadzhiWord.Models.Implementations.StampCollections;
using GadzhiWord.Word.Implementations.Elements;
using GadzhiWord.Word.Interfaces.Elements;
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
                                                    Where(tableElement => CheckFooterIsStamp(tableElement)).
                                                    Select(tableElement => new StampMainWord(tableElement, PaperSize, OrientationType));

        /// <summary>
        /// Проверить является ли колонтитул штампом
        /// </summary>
        private bool CheckFooterIsStamp(ITableElement tableElement) => tableElement.CellsElementWord.
                                                                       Where(cell => !String.IsNullOrWhiteSpace(cell?.Text)).
                                                                       Select(cell => StringAdditionalExtensions.PrepareCellTextToCompare(cell?.Text)).
                                                                       Any(cellText => StampSettingsWord.MarkersMainStamp.MarkerContain(cellText));

        /// <summary>
        /// Преобразовать к виду родительского элемента
        /// </summary>      
        public IOwnerWord ToOwnerWord() => new OwnerWord(this);
    }
}
