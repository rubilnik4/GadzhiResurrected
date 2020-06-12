using System;
using System.Linq;
using GadzhiApplicationCommon.Extensions.Functional.Result;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Implementation.StampCollections.StampPartial.SignatureCreatingPartial;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Fields;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Signatures;

namespace GadzhiApplicationCommon.Models.Implementation.StampCollections.Fields
{
    /// <summary>
    /// Поля штампа, отвечающие за подписи
    /// </summary>
    public class StampSignatureFields : IStampSignatureFields
    {
        public StampSignatureFields(SignaturesBuilder signaturesBuilder)
        {
            if (signaturesBuilder == null) throw new ArgumentNullException(nameof(signaturesBuilder));

            StampPersons = signaturesBuilder.StampPersons;
            StampChanges = signaturesBuilder.StampChanges;
            StampApprovals = signaturesBuilder.StampApprovals;
            StampApprovalsChange = signaturesBuilder.StampApprovalsChange;
            StampApprovalsPerformers = signaturesBuilder.StampApprovalsPerformers;
            StampApprovalsChief = signaturesBuilder.StampApprovalsChief;
        }

        /// <summary>
        /// Строки с ответственным лицом и подписью
        /// </summary>
        public IResultAppCollection<IStampPerson> StampPersons { get; }

        /// <summary>
        /// Строки изменений
        /// </summary>
        public IResultAppCollection<IStampChange> StampChanges { get; }

        /// <summary>
        /// Строки согласования
        /// </summary>
        public IResultAppCollection<IStampApproval> StampApprovals { get; }

        /// <summary>
        /// Строки согласования для извещения с изменениями
        /// </summary>
        public IResultAppCollection<IStampApprovalChange> StampApprovalsChange { get; }

        /// <summary>
        /// Строки согласования для опросных листов и тех требований
        /// </summary>
        public IResultAppCollection<IStampApprovalPerformers> StampApprovalsPerformers { get; }

        /// <summary>
        /// Строки согласования для опросных листов и тех требований с директорами
        /// </summary>
        public IResultAppCollection<IStampApprovalChief> StampApprovalsChief { get; }

        /// <summary>
        /// Получить все подписи
        /// </summary>        
        public IResultAppCollection<IStampSignature> GetSignatures() =>
            StampPersons.Cast<IStampPerson, IStampSignature>().
            ConcatResult(StampChanges.Cast<IStampChange, IStampSignature>()).
            ConcatResult(StampApprovals.Cast<IStampApproval, IStampSignature>()).
            ConcatResult(StampApprovalsChange.Cast<IStampApprovalChange, IStampSignature>()).
            ConcatResult(StampApprovalsPerformers.Cast<IStampApprovalPerformers, IStampSignature>()).
            ConcatResult(StampApprovalsChief.Cast<IStampApprovalChief, IStampSignature>()).
            ResultValueOk(signatures => signatures.Where(signature => signature.IsAbleToInsert)).
            ToResultCollection();
    }
}