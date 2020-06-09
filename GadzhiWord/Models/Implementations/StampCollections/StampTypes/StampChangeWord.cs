using System;
using System.Linq;
using GadzhiApplicationCommon.Extensions.Functional;
using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Implementation.LibraryData;
using GadzhiApplicationCommon.Models.Implementation.StampCollections.Fields;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.LibraryData;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Fields;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Signatures;
using GadzhiWord.Models.Implementations.StampCollections.Fields;
using GadzhiWord.Models.Implementations.StampCollections.Signatures;
using GadzhiWord.Models.Implementations.StampCollections.StampPartial;
using GadzhiWord.Models.Implementations.StampFieldIndexes;
using GadzhiWord.Word.Interfaces.Word.Elements;

namespace GadzhiWord.Models.Implementations.StampCollections.StampTypes
{
    /// <summary>
    /// Поля штампа извещения Word
    /// </summary>
    public class StampChangeWord : StampWord, IStampChangeNotice
    {
        public StampChangeWord(StampSettingsWord stampSettingsWord, SignaturesSearching signaturesSearching, ITableElementWord tableStamp,
                               IResultAppValue<IStampTextField> fullCode)
            : base(stampSettingsWord, signaturesSearching, tableStamp)
        {
            FullCode = fullCode ?? throw new ArgumentNullException(nameof(fullCode));
        }

        /// <summary>
        /// Тип штампа
        /// </summary>
        public override StampType StampType => StampType.ChangeNotice;

        /// <summary>
        /// Поля штампа, отвечающие за подписи
        /// </summary>
        protected override IStampSignatureFields GetStampSignatureFields() =>
            GetStampPersonRows().
            Map(personRows => new StampSignatureFields(personRows,
                                                       new ResultAppCollection<IStampChange>(Enumerable.Empty<IStampChange>()),
                                                       new ResultAppCollection<IStampApproval>(Enumerable.Empty<IStampApproval>()),
                                                       GetStampApprovalChangeRows(),
                                                       new ResultAppCollection<IStampApprovalPerformers>(Enumerable.Empty<IStampApprovalPerformers>())));

        /// <summary>
        /// Получить поле шифра
        /// </summary>
        protected override IResultAppValue<IStampTextField> FullCode { get; }
    }
}