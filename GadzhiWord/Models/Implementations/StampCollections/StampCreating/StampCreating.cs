using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Text;
using System.Linq;
using GadzhiApplicationCommon.Extensions.Functional.Result;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Implementation.FilesConvert;
using GadzhiApplicationCommon.Models.Implementation.LibraryData;
using GadzhiApplicationCommon.Models.Implementation.StampCollections;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.LibraryData;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Fields;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Signatures;
using GadzhiWord.Models.Implementations.StampCollections.Fields;
using GadzhiWord.Models.Implementations.StampCollections.StampTypes;
using GadzhiWord.Word.Interfaces.Word.Elements;

namespace GadzhiWord.Models.Implementations.StampCollections.StampCreating
{
    public static class StampCreating
    {
        /// <summary>
        /// Выбрать главный штамп
        /// </summary>
        public static IStamp GetMainStamp(StampType stampType, ITableElementWord tableWord, StampSettingsWord stampSettings,
                                          SignaturesSearching signaturesSearching, IEnumerable<ITableElementWord> tables) =>
            stampType switch
            {
                StampType.Full => GetFullStamp(stampSettings, signaturesSearching, tableWord, tables),
                StampType.ChangeNotice => GetChangeNoticeStamp(stampSettings, signaturesSearching, tableWord, tables),
                _ => throw new InvalidEnumArgumentException(nameof(stampType), (int)stampType, typeof(StampType)),
            };

        /// <summary>
        /// Получить основной штамп
        /// </summary>
        public static IStamp GetFullStamp(StampSettingsWord stampSettings, SignaturesSearching signaturesSearching, 
                                          ITableElementWord tableWord, IEnumerable<ITableElementWord> tables) =>
            new StampFullWord(stampSettings, signaturesSearching, tableWord, GetApprovalPerformersTable(tables));

        /// <summary>
        /// Получить сокращенные штампы
        /// </summary>
        public static IResultAppCollection<IStamp> GetShortStamps(IStamp mainStamp, IEnumerable<ITableElementWord> tablesWord,
                                                                  StampSettingsWord stampSettings, SignaturesSearching signaturesSearching) =>
            mainStamp.StampSignatureFields.StampPersons.
            ResultValueContinue(persons => persons.Count > 0,
                okFunc: persons => persons[0].SignatureLibrary,
                badFunc: _ => new ErrorApplication(ErrorApplicationType.SignatureNotFound, "Подписи основного штампа не инициализированы")).
            ResultValueOk(personMainSignature =>
                tablesWord.Select((tableWord, index) => GetShortStamp(tableWord,
                                                                      GetStampSettingsWord(index + stampSettings.Id.StampIndex, stampSettings),
                                                                      personMainSignature, signaturesSearching))).
            ToResultCollection();

        /// <summary>
        /// Получить сокращенный штамп
        /// </summary>
        public static IStamp GetShortStamp(ITableElementWord tableWord, StampSettingsWord stampSettings,
                                           ISignatureLibraryApp personMainSignature, SignaturesSearching signaturesSearching) =>
            new StampShortWord(stampSettings, signaturesSearching, tableWord, personMainSignature);


        /// <summary>
        /// Получить параметры штампа Word
        /// </summary>
        public static StampSettingsWord GetStampSettingsWord(int stampIndex, StampSettingsWord stampSettings) =>
            GetStampSettingsWord(stampIndex, stampSettings, stampSettings.PaperSize, stampSettings.Orientation);

        /// <summary>
        /// Получить основной штамп
        /// </summary>
        public static IStamp GetChangeNoticeStamp(StampSettingsWord stampSettings, SignaturesSearching signaturesSearching, 
                                                  ITableElementWord tableWord, IEnumerable<ITableElementWord> tables) =>
            new StampChangeWord(stampSettings, signaturesSearching, tableWord, GetChangeFullCode(tables));

        /// <summary>
        /// Получить параметры штампа Word
        /// </summary>
        public static StampSettingsWord GetStampSettingsWord(int stampIndex, ConvertingSettingsApp convertingSettings,
                                                             string paperSize, StampOrientationType stampOrientationType) =>
            new StampSettingsWord(new StampIdentifier(stampIndex), convertingSettings.PersonId,
                                  convertingSettings.PdfNamingType, paperSize, stampOrientationType);

        /// <summary>
        /// Найти поле основного шифра в таблицах документа
        /// </summary>
        private static IResultAppValue<IStampTextField> GetChangeFullCode(IEnumerable<ITableElementWord> tables)
        {
            ICellElementWord fullCode = null;
            foreach (var table in tables)
            {
                fullCode = table.CellsElementWord.
                              FirstOrDefault(cell => CheckFieldType.IsFieldFullCode(cell, table));

               if (fullCode != null) break;
            }
            return new ResultAppValue<IStampTextField>(new StampTextFieldWord(fullCode, StampFieldType.FullRow) ,
                                                        new ErrorApplication(ErrorApplicationType.FieldNotFound, "Поле шифра в таблице не найдено"));
        }

        /// <summary>
        /// Найти таблицу согласования списка исполнителей
        /// </summary>
        private static IResultAppValue<ITableElementWord> GetApprovalPerformersTable(IEnumerable<ITableElementWord> tables) =>
           new ResultAppValue<ITableElementWord>(tables.FirstOrDefault(CheckFieldType.IsTableApprovalsPerformers),
                                                 new ErrorApplication(ErrorApplicationType.TableNotFound, "Таблица согласования списка исполнителей не найдена"));
    }
}