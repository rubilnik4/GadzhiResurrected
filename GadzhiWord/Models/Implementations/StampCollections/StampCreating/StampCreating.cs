using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Printing;
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
                                          SignaturesSearching signaturesSearching) =>
            stampType switch
            {
                StampType.Full => new StampFullWord(stampSettings, signaturesSearching, tableWord),
                StampType.ChangeNotice => new StampChangeWord(stampSettings, signaturesSearching, tableWord),
                _ => throw new InvalidEnumArgumentException(nameof(stampType), (int)stampType, typeof(StampType)),
            };

        /// <summary>
        /// Получить основной штамп
        /// </summary>
        public static IStamp GetFullStamp(ITableElementWord tableWord, StampSettingsWord stampSettings, SignaturesSearching signaturesSearching) =>
            new StampFullWord(stampSettings, signaturesSearching, tableWord);

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
        public static IStamp GetChangeNoticeStamp(ITableElementWord tableWord, StampSettingsWord stampSettings, SignaturesSearching signaturesSearching) =>
            new StampChangeWord(stampSettings, signaturesSearching, tableWord);

        /// <summary>
        /// Получить параметры штампа Word
        /// </summary>
        public static StampSettingsWord GetStampSettingsWord(int stampIndex, ConvertingSettingsApp convertingSettings,
                                                             string paperSize, StampOrientationType stampOrientationType) =>
            new StampSettingsWord(new StampIdentifier(stampIndex), convertingSettings.PersonId,
                                  convertingSettings.PdfNamingType, paperSize, stampOrientationType);
    }
}