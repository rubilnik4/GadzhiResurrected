using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Signatures;

namespace GadzhiApplicationCommon.Models.Interfaces.StampCollections.Fields
{
    /// <summary>
    /// Поля штампа, отвечающие за подписи
    /// </summary>
    public interface IStampSignatureFields
    {
        /// <summary>
        /// Строки с ответственным лицом и подписью
        /// </summary>
        IResultAppCollection<IStampPerson> StampPersons { get; }

        /// <summary>
        /// Строки изменений
        /// </summary>
        IResultAppCollection<IStampChange> StampChanges { get; }

        /// <summary>
        /// Строки согласования
        /// </summary>
        IResultAppCollection<IStampApproval> StampApprovals { get; }

        /// <summary>
        /// Строки согласования для извещения с изменениями
        /// </summary>
        IResultAppCollection<IStampApprovalChange> StampApprovalsChange { get; }

        /// <summary>
        /// Строки согласования для опросных листов и тех требований
        /// </summary>
        IResultAppCollection<IStampApprovalPerformers> StampApprovalsPerformers { get; }

        /// <summary>
        /// Получить все подписи
        /// </summary>        
        IResultAppCollection<IStampSignature> GetSignatures();
    }
}