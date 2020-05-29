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
using GadzhiApplicationCommon.Models.Implementation.FilesConvert;
using GadzhiApplicationCommon.Models.Implementation.StampCollections;
using GadzhiWord.Extensions.StringAdditional;
using GadzhiWord.Models.Implementations.StampCollections.StampMainPartial;
using GadzhiApplicationCommon.Extensions.Functional;

namespace GadzhiWord.Word.Implementations.DocumentWordPartial
{
    /// <summary>
    /// Подкласс документа Word для работы с элементами
    /// </summary>
    public partial class DocumentWord
    {
        /// <summary>
        /// Загруженные штампы
        /// </summary>
        private IStampContainer _stampContainer;

        /// <summary>
        /// Найти все штампы во всех моделях и листах
        /// </summary>       
        public IStampContainer GetStampContainer(ConvertingSettingsApplication convertingSettings) =>
            _stampContainer ??= new StampContainer(FindStamps(convertingSettings), FullName);

        /// <summary>
        /// Найти таблицы в колонтитулах
        /// </summary>
        private IEnumerable<IStamp> FindStamps(ConvertingSettingsApplication convertingSettings) =>
            _document.Sections.ToIEnumerable().
            SelectMany(section => section.Footers.ToIEnumerable()).
            SelectMany(footer => footer.Range.Tables.ToIEnumerable()).
            Select(table => new TableElementWord(table, ToOwnerWord())).
            Where(CheckFooterIsStamp).
            Select((tableElement, stampIndex) => 
                       new StampSettingsWord(new StampIdentifier(stampIndex), convertingSettings.PersonId, 
                                             convertingSettings.PdfNamingType, PaperSize, OrientationType).
                       Map(stampSettings => new StampMainWord(stampSettings, ApplicationWord.ResourcesWord.SignaturesSearching, tableElement)));

        /// <summary>
        /// Проверить является ли колонтитул штампом
        /// </summary>
        private static bool CheckFooterIsStamp(ITableElementWord tableElement) => tableElement.CellsElementWord.
                                                                       Where(cell => !String.IsNullOrWhiteSpace(cell?.Text)).
                                                                       Select(cell => cell.Text.PrepareCellTextToCompare()).
                                                                       Any(cellText => AdditionalSettingsWord.MarkersMainStamp.MarkerContain(cellText));

        /// <summary>
        /// Преобразовать к виду родительского элемента
        /// </summary>      
        public IOwnerWord ToOwnerWord() => new OwnerWord(this);
    }
}
