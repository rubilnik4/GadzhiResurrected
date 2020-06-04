using System.Collections.Generic;
using System.Linq;
using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiApplicationCommon.Models.Implementation.LibraryData;
using GadzhiApplicationCommon.Models.Implementation.StampCollections.Fields;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Fields;
using GadzhiCommon.Extensions.Functional;
using GadzhiWord.Models.Implementations.StampCollections.StampPartial;
using GadzhiWord.Word.Interfaces.Word.Elements;

namespace GadzhiWord.Models.Implementations.StampCollections.StampMainPartial
{
    /// <summary>
    /// Поля основного штампа Word
    /// </summary>
    public class StampMainWord : StampWord, IStampMain
    {
        public StampMainWord(StampSettingsWord stampSettingsWord, SignaturesSearching signaturesSearching, ITableElementWord tableStamp)
            : base(stampSettingsWord, signaturesSearching, tableStamp)
        { }

        /// <summary>
        /// Тип штампа
        /// </summary>
        public override StampType StampType => StampType.Main;

        /// <summary>
        /// Поля штампа, отвечающие за подписи
        /// </summary>
        protected override IStampSignatureFields GetStampSignatureFields() =>
            GetStampPersonRows().
            Map(personRows => new StampSignatureFields(personRows, GetStampChangeRows(personRows.Value?.FirstOrDefault()?.SignatureLibrary)));
    }
}
