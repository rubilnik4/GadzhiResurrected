using System.Linq;
using GadzhiApplicationCommon.Extensions.Functional;
using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiApplicationCommon.Models.Implementation.LibraryData;
using GadzhiApplicationCommon.Models.Implementation.StampCollections.Fields;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Fields;
using GadzhiWord.Models.Implementations.StampCollections.StampPartial;
using GadzhiWord.Word.Interfaces.Word.Elements;

namespace GadzhiWord.Models.Implementations.StampCollections.StampTypes
{
    /// <summary>
    /// Поля штампа извещения Word
    /// </summary>
    public class StampChangeNoticeWord : StampWord, IStampChangeNotice
    {
        public StampChangeNoticeWord(StampSettingsWord stampSettingsWord, SignaturesSearching signaturesSearching, ITableElementWord tableStamp)
            : base(stampSettingsWord, signaturesSearching, tableStamp)
        { }

        /// <summary>
        /// Тип штампа
        /// </summary>
        public override StampType StampType => StampType.ChangeNotice;

        /// <summary>
        /// Поля штампа, отвечающие за подписи
        /// </summary>
        protected override IStampSignatureFields GetStampSignatureFields() =>
            GetStampPersonRows().
            Map(personRows => new StampSignatureFields(personRows));
    }
}