using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using GadzhiApplicationCommon.Extensions.Functional;
using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiApplicationCommon.Models.Implementation.FilesConvert;
using GadzhiApplicationCommon.Models.Implementation.StampCollections;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiWord.Extensions.StringAdditional;
using GadzhiWord.Extensions.Word;
using GadzhiWord.Models.Implementations.StampCollections;
using GadzhiWord.Models.Implementations.StampCollections.StampMainPartial;
using GadzhiWord.Word.Implementations.Word.Elements;
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
            Select(GetTableStamp).
            Where(tableStamp => tableStamp.StampType != StampType.Unknown).
            Select((tableStamp, stampIndex) => GetStampByType(tableStamp.TableWord, tableStamp.StampType,
                                                              GetStampSettingsWord(stampIndex, convertingSettings)));

        /// <summary>
        /// Получить таблицу и тип соответствующего штампа
        /// </summary>
        private static (ITableElementWord TableWord, StampType StampType) GetTableStamp(ITableElementWord tableWord) =>
            (tableWord, StampMarkersWord.GetStampType(tableWord));

        /// <summary>
        /// Получить параметры штампа Word
        /// </summary>
        private StampSettingsWord GetStampSettingsWord(int stampIndex, ConvertingSettingsApplication convertingSettings) =>
            new StampSettingsWord(new StampIdentifier(stampIndex), convertingSettings.PersonId, 
                                  convertingSettings.PdfNamingType, PaperSize, OrientationType);

        /// <summary>
        /// Получить штамп из таблицы по типу
        /// </summary>
        private IStamp GetStampByType(ITableElementWord tableWord, StampType stampType, StampSettingsWord stampSettings) =>
            stampType switch
            {
                StampType.Main => new StampMainWord(stampSettings, ApplicationOffice.ResourcesWord.SignaturesSearching, tableWord),
                StampType.Shortened => new StampMainWord(stampSettings, ApplicationOffice.ResourcesWord.SignaturesSearching, tableWord),
                _ => throw new InvalidEnumArgumentException(nameof(stampType), (int)stampType, typeof(StampType))
            };

        /// <summary>
        /// Получить таблицы
        /// </summary>
        private IReadOnlyList<ITableElementWord> GetTables() =>
            _document.Tables.ToEnumerable().
            Select(table => new TableElementWord(table, ToOwnerWord)).
            ToList();
    }
}
