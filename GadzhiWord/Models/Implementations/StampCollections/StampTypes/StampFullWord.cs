using System;
using System.Linq;
using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Implementation.LibraryData;
using GadzhiApplicationCommon.Models.Implementation.StampCollections.Fields;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Fields;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Signatures;
using GadzhiCommon.Extensions.Functional;
using GadzhiWord.Models.Implementations.StampCollections.StampPartial;
using GadzhiWord.Word.Interfaces.Word.Elements;

namespace GadzhiWord.Models.Implementations.StampCollections.StampTypes
{
    /// <summary>
    /// Поля основного штампа Word
    /// </summary>
    public class StampFullWord : StampWord, IStampFull
    {
        public StampFullWord(StampSettingsWord stampSettingsWord, SignaturesSearching signaturesSearching,
                             ITableElementWord tableStamp, IResultAppValue<ITableElementWord> tableApprovalPerformers)
            : base(stampSettingsWord, signaturesSearching, tableStamp)
        {
            TableApprovalPerformers = tableApprovalPerformers ?? throw new ArgumentNullException(nameof(tableApprovalPerformers));
        }

        /// <summary>
        /// Элемент таблица согласования списка исполнителей
        /// </summary>
        protected override IResultAppValue<ITableElementWord> TableApprovalPerformers { get; }

        /// <summary>
        /// Тип штампа
        /// </summary>
        public override StampType StampType => StampType.Full;

        /// <summary>
        /// Поля штампа, отвечающие за подписи
        /// </summary>
        protected override IStampSignatureFields GetStampSignatureFields() =>
            GetStampPersonRows().
            Map(personRows => new StampSignatureFields(personRows,
                                                       GetStampChangeRows(personRows.Value?.FirstOrDefault()?.SignatureLibrary),
                                                       new ResultAppCollection<IStampApproval>(Enumerable.Empty<IStampApproval>()),
                                                       new ResultAppCollection<IStampApprovalChange>(Enumerable.Empty<IStampApprovalChange>()),
                                                       GetStampApprovalPerformersRows()));

    }
}
