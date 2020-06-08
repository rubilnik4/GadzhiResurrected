using System;
using System.Collections.Generic;
using System.Linq;
using GadzhiApplicationCommon.Extensions.Functional;
using GadzhiApplicationCommon.Extensions.Functional.Result;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Implementation.FilesConvert;
using GadzhiApplicationCommon.Models.Implementation.StampCollections;
using GadzhiApplicationCommon.Models.Implementation.StampCollections.StampContainer;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.LibraryData;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiWord.Extensions.Word;
using GadzhiWord.Models.Implementations.StampCollections;
using GadzhiWord.Models.Implementations.StampCollections.StampCreating;
using GadzhiWord.Models.Implementations.StampCollections.StampTypes;
using GadzhiWord.Word.Implementations.Word.Elements;
using GadzhiWord.Word.Implementations.Word.Interop;
using GadzhiWord.Word.Interfaces.Word.Elements;
using Microsoft.Office.Interop.Word;

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
        /// Список штампов
        /// </summary>
        private IStampContainer _stampContainer;

        /// <summary>
        /// Список штампов
        /// </summary>
        public IStampContainer GetStampContainer(ConvertingSettingsApp convertingSettings) =>
            _stampContainer ??= new StampContainer(FindStamps(convertingSettings), StampContainerType.United,
                                                   StampApplicationType.Word);

        /// <summary>
        /// Преобразовать к виду родительского элемента
        /// </summary>      
        public IOwnerWord ToOwnerWord => new OwnerWord(this);

        /// <summary>
        /// Найти таблицы в колонтитулах
        /// </summary>
        private IResultAppCollection<IStamp> FindStamps(ConvertingSettingsApp convertingSettings) =>
            _document.Sections.ToEnumerable().
            SelectMany(section => section.Footers.ToEnumerable()).
            SelectMany(SearchInRange.GetTablesFromFooter).
            Select(table => new TableElementWord(table, ToOwnerWord)).
            Select(GetTableStamp).
            Where(tableStamp => tableStamp.StampType != StampType.Unknown).
            OrderBy(tableStamp => tableStamp.StampType).
            Map(StampValidatingWord.ValidateTableStampsByType).
            ResultValueOkBind(tableStamp => GetStampsInOrder(tableStamp, convertingSettings)).
            ToResultCollection();

        /// <summary>
        /// Получить таблицу и тип соответствующего штампа
        /// </summary>
        private static TableStampType GetTableStamp(ITableElementWord tableWord) =>
            new TableStampType(StampMarkersWord.GetStampType(tableWord), tableWord);

        /// <summary>
        /// Поучить штампы в порядке их сортировки
        /// </summary>
        private IResultAppCollection<IStamp> GetStampsInOrder(IList<TableStampType> tablesStamp, ConvertingSettingsApp convertingSettings) =>
            StampCreating.GetMainStamp(tablesStamp[0].StampType, tablesStamp[0].TableWord,
                                       StampCreating.GetStampSettingsWord(0, convertingSettings, PaperSize, OrientationType),
                                       ApplicationOffice.ResourcesWord.SignaturesSearching).
            Map(mainStamp => new ResultAppCollection<IStamp>(new List<IStamp>() { mainStamp }).
                             ConcatResult(GetShortStamps(tablesStamp.Select(tableStamp => tableStamp.TableWord),
                                                         mainStamp,  convertingSettings)));

        /// <summary>
        /// Получить сокращенные штампы
        /// </summary>
        private IResultAppCollection<IStamp> GetShortStamps(IEnumerable<ITableElementWord> tablesStamp, IStamp fullStamp, ConvertingSettingsApp convertingSettings) =>
            StampCreating.GetShortStamps(fullStamp, tablesStamp.Skip(1),
                                         StampCreating.GetStampSettingsWord(1, convertingSettings, PaperSize, OrientationType),
                                         ApplicationOffice.ResourcesWord.SignaturesSearching);

        /// <summary>
        /// Получить таблицы
        /// </summary>
        private IReadOnlyList<ITableElementWord> GetTables() =>
            _document.Tables.ToEnumerable().
            Select(table => new TableElementWord(table, ToOwnerWord)).
            ToList();
    }
}
