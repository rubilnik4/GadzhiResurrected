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
                  new ResultAppCollection<IStampApprovalPerformers>(Enumerable.Empty<IStampApprovalPerformers>()),
                  new ResultAppCollection<IStampApprovalChief>(Enumerable.Empty<IStampApprovalChief>()))
        { }

        public SignaturesBuilder(IResultAppCollection<IStampPerson> stampPersons, IResultAppCollection<IStampChange> stampChanges,
                                 IResultAppCollection<IStampApproval> stampApproval, IResultAppCollection<IStampApprovalChange> stampApprovalsChange,
                                 IResultAppCollection<IStampApprovalPerformers> stampApprovalsPerformers,
                                 IResultAppCollection<IStampApprovalChief> stampApprovalsChief)
        {
            StampPersons = stampPersons ?? throw new ArgumentNullException(nameof(stampPersons));
            StampChanges = stampChanges ?? throw new ArgumentNullException(nameof(stampChanges));
            StampApprovals = stampApproval ?? throw new ArgumentNullException(nameof(stampApproval));
            StampApprovalsChange = stampApprovalsChange ?? throw new ArgumentNullException(nameof(stampApprovalsChange));
            StampApprovalsPerformers = stampApprovalsPerformers ?? throw new ArgumentNullException(nameof(stampApprovalsPerformers));
            StampApprovalsChief = stampApprovalsChief ?? throw new ArgumentNullException(nameof(stampApprovalsChief));
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
        /// Строки согласования без подписи для опросных листов и тех требований с директорами
        /// </summary>
        public IResultAppCollection<IStampApprovalChief> StampApprovalsChief { get; }

        /// <summary>
        /// Добавить строки с ответственным лицом
        /// </summary>
        public SignaturesBuilder AddStampPersons(IResultAppCollection<IStampPerson> stampPersons) =>
            new SignaturesBuilder(stampPersons, StampChanges, StampApprovals, StampApprovalsChange, StampApprovalsPerformers, StampApprovalsChief);

        /// <summary>
        /// Добавить строки изменений
        /// </summary>
        public SignaturesBuilder AddStampChanges(IResultAppCollection<IStampChange> stampChanges) =>
            new SignaturesBuilder(StampPersons, stampChanges, StampApprovals, StampApprovalsChange, StampApprovalsPerformers, StampApprovalsChief);

        /// <summary>
        /// Добавить строки согласования
        /// </summary>
        public SignaturesBuilder AddStampApprovals(IResultAppCollection<IStampApproval> stampApprovals) =>
            new SignaturesBuilder(StampPersons, StampChanges, stampApprovals, StampApprovalsChange, StampApprovalsPerformers, StampApprovalsChief);

        /// <summary>
        /// Добавить строки согласования для извещения с изменениями
        /// </summary>
        public SignaturesBuilder AddStampApprovalsChange(IResultAppCollection<IStampApprovalChange> stampApprovalsChange) =>
            new SignaturesBuilder(StampPersons, StampChanges, StampApprovals, stampApprovalsChange, StampApprovalsPerformers, StampApprovalsChief);

        /// <summary>
        /// Добавить строки согласования для опросных листов и тех требований
        /// </summary>
        public SignaturesBuilder AddStampApprovalsPerformers(IResultAppCollection<IStampApprovalPerformers> stampApprovalsPerformers) =>
            new SignaturesBuilder(StampPersons, StampChanges, StampApprovals, StampApprovalsChange, stampApprovalsPerformers, StampApprovalsChief);

        /// <summary>
        /// Добавить строки согласования для опросных листов и тех требований с директорами
        /// </summary>
        public SignaturesBuilder AddStampApprovalsChief(IResultAppCollection<IStampApprovalChief> stampApprovalChief) =>
            new SignaturesBuilder(StampPersons, StampChanges, StampApprovals, StampApprovalsChange, StampApprovalsPerformers, stampApprovalChief);
    }
}