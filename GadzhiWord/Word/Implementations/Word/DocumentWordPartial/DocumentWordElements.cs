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
        /// Список штампов
        /// </summary>
        private IStampContainer _stampContainer;

        /// <summary>
        /// Список штампов
        /// </summary>
        public IStampContainer GetStampContainer(ConvertingSettingsApplication convertingSettings) =>
            _stampContainer ??= new StampContainer(FindStamps(convertingSettings), StampContainerType.United, 
                                                   StampApplicationType.Word);

        /// <summary>
        /// Преобразовать к виду родительского элемента
        /// </summary>      
        public IOwnerWord ToOwnerWord => new OwnerWord(this);

        /// <summary>
        /// Найти таблицы в колонтитулах
        /// </summary>
        private IResultAppCollection<IStamp> FindStamps(ConvertingSettingsApplication convertingSettings) =>
            _document.Sections.ToEnumerable().
            SelectMany(section => section.Footers.ToEnumerable()).
            SelectMany(footer => footer.Range.Tables.ToEnumerable()).
            Select(table => new TableElementWord(table, ToOwnerWord)).
            Select(GetTableStamp).
            Where(tableStamp => tableStamp.StampType != StampType.Unknown).
            OrderBy(tableStamp => tableStamp.StampType).
            Map(ValidateTableStampsByType).
            ResultValueOkBind(tablesWord => GetStampsInOrder(tablesWord, convertingSettings)).
            ToResultCollection();

        /// <summary>
        /// Получить таблицу и тип соответствующего штампа
        /// </summary>
        private static (ITableElementWord TableWord, StampType StampType) GetTableStamp(ITableElementWord tableWord) =>
            (tableWord, StampMarkersWord.GetStampType(tableWord));

        /// <summary>
        /// Проверить штампы на наличие основного типа
        /// </summary>
        private static IResultAppCollection<ITableElementWord> ValidateTableStampsByType(IEnumerable<(ITableElementWord, StampType)> tablesStampWord) =>
            new ResultAppCollection<(ITableElementWord TableWord, StampType StampType)>(tablesStampWord, new ErrorApplication(ErrorApplicationType.StampNotFound,
                                                                                                                              "Штампы не найдены")).
            ResultValueContinue(tablesStamp => tablesStamp[0].StampType == StampType.Main,
                okFunc: tablesStamp => tablesStamp,
                badFunc: _ => new ErrorApplication(ErrorApplicationType.StampNotFound, "Основной штамп не найден")).
            ResultValueOk(tablesStamp => tablesStamp.Select (tableStamp => tableStamp.TableWord)).
            ToResultCollection();

        /// <summary>
        /// Поучить штампы в порядке их сортировки
        /// </summary>
        private IResultAppCollection<IStamp> GetStampsInOrder(IList<ITableElementWord> tablesStamp, ConvertingSettingsApplication convertingSettings) =>
            GetMainStamp(tablesStamp[0], GetStampSettingsWord(0, convertingSettings)).
            Map(mainStamp => new ResultAppCollection<IStamp>(new List<IStamp>() { mainStamp }).
                             ConcatResult(GetShortStamps(mainStamp, tablesStamp.Skip(1), convertingSettings, 1)));

        /// <summary>
        /// Получить основной штамп
        /// </summary>
        private IStamp GetMainStamp(ITableElementWord tableWord, StampSettingsWord stampSettings) =>
            new StampMainWord(stampSettings, ApplicationOffice.ResourcesWord.SignaturesSearching, tableWord);

        /// <summary>
        /// Получить сокращенные штампы
        /// </summary>
        private IResultAppCollection<IStamp> GetShortStamps(IStamp mainStamp, IEnumerable<ITableElementWord> tablesWord,
                                                            ConvertingSettingsApplication convertingSettings, int startIndex) =>
            mainStamp.StampSignatureFields.StampPersons.
            ResultValueContinue(persons => persons.Count > 0,
                okFunc: persons => persons[0].SignatureLibrary,
                badFunc: _ => new ErrorApplication(ErrorApplicationType.SignatureNotFound, "Подписи основного штампа не инициализированы")).
            ResultValueOk(personMainSignature =>
                tablesWord.Select((tableWord, index) => GetShortStamp(tableWord, GetStampSettingsWord(index + startIndex, convertingSettings),
                                                                     personMainSignature))).
            ToResultCollection();

        /// <summary>
        /// Получить сокращенный штамп
        /// </summary>
        private IStamp GetShortStamp(ITableElementWord tableWord, StampSettingsWord stampSettings, ISignatureLibraryApp personMainSignature) =>
            new StampShortWord(stampSettings, ApplicationOffice.ResourcesWord.SignaturesSearching, tableWord, personMainSignature);

        /// <summary>
        /// Получить параметры штампа Word
        /// </summary>
        private StampSettingsWord GetStampSettingsWord(int stampIndex, ConvertingSettingsApplication convertingSettings) =>
            new StampSettingsWord(new StampIdentifier(stampIndex), convertingSettings.PersonId,
                                  convertingSettings.PdfNamingType, PaperSize, OrientationType);


        /// <summary>
        /// Получить таблицы
        /// </summary>
        private IReadOnlyList<ITableElementWord> GetTables() =>
            _document.Tables.ToEnumerable().
            Select(table => new TableElementWord(table, ToOwnerWord)).
            ToList();
    }
}
