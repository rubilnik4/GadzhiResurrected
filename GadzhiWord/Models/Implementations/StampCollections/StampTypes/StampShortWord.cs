using System;
using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiApplicationCommon.Models.Implementation.LibraryData;
using GadzhiApplicationCommon.Models.Implementation.StampCollections.Fields;
using GadzhiApplicationCommon.Models.Implementation.StampCollections.StampPartial.SignatureCreatingPartial;
using GadzhiApplicationCommon.Models.Interfaces.LibraryData;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Fields;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.StampTypes;
using GadzhiWord.Models.Implementations.StampCollections.StampPartial;
using GadzhiWord.Word.Interfaces.Word.Elements;

namespace GadzhiWord.Models.Implementations.StampCollections.StampTypes
{
    /// <summary>
    /// Поля сокращенного штампа Word
    /// </summary>
    public class StampShortWord : StampWord, IStampShort
    {
        /// <summary>
        /// Подпись сокращенного штампа
        /// </summary>
        private readonly ISignatureLibraryApp _personShortSignature;

        public StampShortWord(StampSettingsWord stampSettingsWord, SignaturesSearching signaturesSearching,
                              ITableElementWord tableStamp, ISignatureLibraryApp personShortSignature)
            : base(stampSettingsWord, signaturesSearching, tableStamp)
        {
            _personShortSignature = personShortSignature ?? throw new ArgumentNullException(nameof(personShortSignature));
        }

        /// <summary>
        /// Тип штампа
        /// </summary>
        public override StampType StampType => StampType.Shortened;

        /// <summary>
        /// Поля штампа, отвечающие за подписи
        /// </summary>
        protected override IStampSignatureFields GetStampSignatureFields() =>
            new StampSignatureFields(new SignaturesBuilder().
                                     AddStampChanges(SignatureCreating.GetStampChangeRows(_personShortSignature)));
    }
}