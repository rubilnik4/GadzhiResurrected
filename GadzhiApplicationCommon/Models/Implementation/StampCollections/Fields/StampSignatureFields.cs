using System;
using System.Linq;
using GadzhiApplicationCommon.Extensions.Functional.Result;
using GadzhiApplicationCommon.Models.Implementation.Errors;
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
        public StampSignatureFields(IResultAppCollection<IStampPerson> stampPersons)
            : this(stampPersons, new ResultAppCollection<IStampChange>(Enumerable.Empty<IStampChange>()))
        { }

        public StampSignatureFields(IResultAppCollection<IStampChange> stampChanges)
          : this(new ResultAppCollection<IStampPerson>(Enumerable.Empty<IStampPerson>()), stampChanges)
        { }

        public StampSignatureFields(IResultAppCollection<IStampPerson> stampPersons, IResultAppCollection<IStampChange> stampChanges)
            : this(stampPersons, stampChanges,
                   new ResultAppCollection<IStampApproval>(Enumerable.Empty<IStampApproval>()),
                   new ResultAppCollection<IStampApprovalChange>(Enumerable.Empty<IStampApprovalChange>()),
                   new ResultAppCollection<IStampApprovalPerformers>(Enumerable.Empty<IStampApprovalPerformers>()))
        { }

        public StampSignatureFields(IResultAppCollection<IStampPerson> stampPersons, IResultAppCollection<IStampChange> stampChanges,
                                    IResultAppCollection<IStampApproval> stampApproval)
            : this(stampPersons, stampChanges, stampApproval,
                   new ResultAppCollection<IStampApprovalChange>(Enumerable.Empty<IStampApprovalChange>()),
                   new ResultAppCollection<IStampApprovalPerformers>(Enumerable.Empty<IStampApprovalPerformers>()))
        { }

        public StampSignatureFields(IResultAppCollection<IStampPerson> stampPersons, IResultAppCollection<IStampChange> stampChanges,
                                    IResultAppCollection<IStampApproval> stampApproval, IResultAppCollection<IStampApprovalChange> stampApprovalChange,
                                    IResultAppCollection<IStampApprovalPerformers> stampApprovalPerformers)
        {
            StampPersons = stampPersons ?? throw new ArgumentNullException(nameof(stampPersons));
            StampChanges = stampChanges ?? throw new ArgumentNullException(nameof(stampChanges));
            StampApproval = stampApproval ?? throw new ArgumentNullException(nameof(stampApproval));
            StampApprovalChange = stampApprovalChange ?? throw new ArgumentNullException(nameof(stampApprovalChange));
            StampApprovalPerformers = stampApprovalPerformers ?? throw new ArgumentNullException(nameof(stampApprovalPerformers));
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
        public IResultAppCollection<IStampApproval> StampApproval { get; }

        /// <summary>
        /// Строки согласования для извещения с изменениями
        /// </summary>
        public IResultAppCollection<IStampApprovalChange> StampApprovalChange { get; }

        /// <summary>
        /// Строки согласования для опросных листов и тех требований
        /// </summary>
        public IResultAppCollection<IStampApprovalPerformers > StampApprovalPerformers { get; }

        /// <summary>
        /// Получить все подписи
        /// </summary>        
        public IResultAppCollection<IStampSignature> GetSignatures() =>
            StampPersons.Cast<IStampPerson, IStampSignature>().
            ConcatResult(StampChanges.Cast<IStampChange, IStampSignature>()).
            ConcatResult(StampApproval.Cast<IStampApproval, IStampSignature>()).
            ConcatResult(StampApprovalChange.Cast<IStampApprovalChange, IStampSignature>()).
            ConcatResult(StampApprovalPerformers.Cast<IStampApprovalPerformers, IStampSignature>()).
            ResultValueOk(signatures => signatures.Where(signature => signature.IsAbleToInsert)).
            ToResultCollection();
    }
}