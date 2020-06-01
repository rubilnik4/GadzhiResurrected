using System;
using System.Collections.Generic;
using System.Linq;
using GadzhiApplicationCommon.Extensions.Functional;
using GadzhiApplicationCommon.Models.Implementation.FilesConvert;
using GadzhiApplicationCommon.Models.Implementation.StampCollections;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiWord.Extensions.StringAdditional;
using GadzhiWord.Extensions.Word;
using GadzhiWord.Models.Implementations.StampCollections;
using GadzhiWord.Models.Implementations.StampCollections.StampMainPartial;
using GadzhiWord.Word.Implementations.Word.Elements;
using GadzhiWord.Word.Interfaces.Word.Elements;

namespace GadzhiWord.Word.Implementations.Word.DocumentWordPartial
{
    /// <summary>
    /// Подкласс документа Word для работы с элементами
    /// </summary>
    public partial class DocumentWord
    {
        /// <summary>
        /// Таблицы
        /// </summary>
        private IReadOnlyList<ITableElementWord> _tables;

        /// <summary>
        /// Таблицы
        /// </summary>
        public IReadOnlyList<ITableElementWord> Tables => _tables ??= GetTables();

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
        /// Преобразовать к виду родительского элемента
        /// </summary>      
        public IOwnerWord ToOwnerWord => new OwnerWord(this);

        /// <summary>
        /// Найти таблицы в колонтитулах
        /// </summary>
        private IEnumerable<IStamp> FindStamps(ConvertingSettingsApplication convertingSettings) =>
            _document.Sections.ToEnumerable().
            SelectMany(section => section.Footers.ToEnumerable()).
            SelectMany(footer => footer.Range.Tables.ToEnumerable()).
            Select(table => new TableElementWord(table, ToOwnerWord)).
            Where(CheckFooterIsStamp).
            Select((tableElement, stampIndex) =>
                       new StampSettingsWord(new StampIdentifier(stampIndex), convertingSettings.PersonId,
                                             convertingSettings.PdfNamingType, PaperSize, OrientationType).
                       Map(stampSettings => new StampMainWord(stampSettings, ApplicationOffice.ResourcesWord.SignaturesSearching, tableElement)));

        /// <summary>
        /// Получить таблицы
        /// </summary>
        private IReadOnlyList<ITableElementWord> GetTables() =>
            _document.Tables.ToEnumerable().
            Select(table => new TableElementWord(table, ToOwnerWord)).
            ToList();
            
        /// <summary>
        /// Проверить является ли колонтитул штампом
        /// </summary>
        private static bool CheckFooterIsStamp(ITableElementWord tableElement) => tableElement.CellsElementWord.
                                                                       Where(cell => !String.IsNullOrWhiteSpace(cell?.Text)).
                                                                       Select(cell => cell.Text.PrepareCellTextToCompare()).
                                                                       Any(cellText => AdditionalSettingsWord.MarkersMainStamp.MarkerContain(cellText));


    }
}
