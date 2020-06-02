using System;
using System.Collections.Generic;
using System.Linq;
using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiApplicationCommon.Models.Implementation.LibraryData;
using GadzhiApplicationCommon.Models.Implementation.StampCollections.Fields;
using GadzhiApplicationCommon.Models.Interfaces.LibraryData;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Fields;
using GadzhiWord.Models.Implementations.StampCollections.StampPartial;
using GadzhiWord.Word.Interfaces.Word.Elements;

namespace GadzhiWord.Models.Implementations.StampCollections.StampMainPartial
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
        public override IStampSignatureFields StampSignatureFields =>
            new StampSignatureFields(GetStampChangeRows(_personShortSignature));
    }
}