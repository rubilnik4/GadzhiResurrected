using System;
using System.Linq;
using GadzhiApplicationCommon.Extensions.Functional;
using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiApplicationCommon.Models.Implementation.LibraryData;
using GadzhiApplicationCommon.Models.Implementation.StampCollections.Fields;
using GadzhiApplicationCommon.Models.Implementation.StampCollections.StampPartial.SignatureCreatingPartial;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Fields;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.StampTypes;
using GadzhiWord.Models.Implementations.StampCollections.StampPartial;
using GadzhiWord.Word.Interfaces.Word.Elements;

namespace GadzhiWord.Models.Implementations.StampCollections.StampTypes
{
    /// <summary>
    /// Поля штампа извещения Word
    /// </summary>
    public class StampChangeWord : StampWord, IStampChangeNotice
    {
        /// <summary>
        /// Поле шифра
        /// </summary>
        private readonly IResultAppValue<IStampTextField> _fullcode;

        public StampChangeWord(StampSettingsWord stampSettingsWord, SignaturesSearching signaturesSearching, ITableElementWord tableStamp,
                               IResultAppValue<IStampTextField> fullCode)
            : base(stampSettingsWord, signaturesSearching, tableStamp)
        {
            _fullcode = fullCode ?? throw new ArgumentNullException(nameof(fullCode));
        }

        /// <summary>
        /// Тип штампа
        /// </summary>
        public override StampType StampType => StampType.ChangeNotice;

        /// <summary>
        /// Поля штампа, отвечающие за подписи
        /// </summary>
        protected override IStampSignatureFields GetStampSignatureFields() =>
            SignatureCreating.GetStampPersonRows().
            Map(personRows => new StampSignatureFields(new SignaturesBuilder().
                                                       AddStampPersons(personRows).
                                                       AddStampApprovalsChange(SignatureCreating.GetStampApprovalChangeRows())));

        /// <summary>
        /// Получить поле шифра
        /// </summary>
        protected override IResultAppValue<IStampTextField> GetFullCode() => _fullcode;
    }
}