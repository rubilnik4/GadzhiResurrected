using System;
using System.Linq;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Signatures;

namespace GadzhiApplicationCommon.Models.Implementation.StampCollections.StampPartial.SignatureCreatingPartial
{
    /// <summary>
    /// Класс-строитель для создания подписей
    /// </summary>
    public class SignaturesBuilder
    {
        public SignaturesBuilder()
            :this(new ResultAppCollection<IStampPerson>(Enumerable.Empty<IStampPerson>()),
                  new ResultAppCollection<IStampChange>(Enumerable.Empty<IStampChange>()),
                  new ResultAppCollection<IStampApproval>(Enumerable.Empty<IStampApproval>()),
                  new ResultAppCollection<IStampApprovalChange>(Enumerable.Empty<IStampApprovalChange>()),
                  new ResultAppCollection<IStampApprovalPerformers>(Enumerable.Empty<IStampApprovalPerformers>()))
        { }

        public SignaturesBuilder(IResultAppCollection<IStampPerson> stampPersons, IResultAppCollection<IStampChange> stampChanges,
                                 IResultAppCollection<IStampApproval> stampApproval, IResultAppCollection<IStampApprovalChange> stampApprovalChange,
                                 IResultAppCollection<IStampApprovalPerformers> stampApprovalPerformers)
        {
            StampPersons = stampPersons ?? throw new ArgumentNullException(nameof(stampPersons));
            StampChanges = stampChanges ?? throw new ArgumentNullException(nameof(stampChanges));
            StampApprovals = stampApproval ?? throw new ArgumentNullException(nameof(stampApproval));
            StampApprovalsChange = stampApprovalChange ?? throw new ArgumentNullException(nameof(stampApprovalChange));
            StampApprovalsPerformers = stampApprovalPerformers ?? throw new ArgumentNullException(nameof(stampApprovalPerformers));
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
        /// Добавить строки с ответственным лицом
        /// </summary>
        public SignaturesBuilder AddStampPersons(IResultAppCollection<IStampPerson> stampPersons) =>
            new SignaturesBuilder(stampPersons, StampChanges, StampApprovals, StampApprovalsChange, StampApprovalsPerformers);

        /// <summary>
        /// Добавить строки изменений
        /// </summary>
        public SignaturesBuilder AddStampChanges(IResultAppCollection<IStampChange> stampChanges) =>
            new SignaturesBuilder(StampPersons, stampChanges, StampApprovals, StampApprovalsChange, StampApprovalsPerformers);

        /// <summary>
        /// Добавить строки согласования
        /// </summary>
        public SignaturesBuilder AddStampApprovals(IResultAppCollection<IStampApproval> stampApprovals) =>
            new SignaturesBuilder(StampPersons, StampChanges, stampApprovals, StampApprovalsChange, StampApprovalsPerformers);

        /// <summary>
        /// Добавить строки согласования для извещения с изменениями
        /// </summary>
        public SignaturesBuilder AddStampApprovalsChange(IResultAppCollection<IStampApprovalChange> stampApprovalsChange) =>
            new SignaturesBuilder(StampPersons, StampChanges, StampApprovals, stampApprovalsChange, StampApprovalsPerformers);

        /// <summary>
        /// Добавить строки согласования для опросных листов и тех требований
        /// </summary>
        public SignaturesBuilder AddStampApprovalsPerformers(IResultAppCollection<IStampApprovalPerformers> stampApprovalsPerformers) =>
            new SignaturesBuilder(StampPersons, StampChanges, StampApprovals, StampApprovalsChange, stampApprovalsPerformers);
    }
}