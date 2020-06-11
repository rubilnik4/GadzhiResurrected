using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.LibraryData;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Signatures;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.StampPartial.SignatureCreating;

namespace GadzhiApplicationCommon.Models.Implementation.StampCollections.StampPartial.SignatureCreatingPartial
{
    /// <summary>
    /// Фабрика создания подписей Word
    /// </summary>
    public abstract class SignatureCreating : ISignatureCreating
    {
        /// <summary>
        /// Получить строки с ответственным лицом без подписи
        /// </summary>
        public abstract IResultAppCollection<IStampPerson> GetStampPersonRows();

        /// <summary>
        /// Получить строки с изменениями
        /// </summary>
        public abstract IResultAppCollection<IStampChange> GetStampChangeRows(ISignatureLibraryApp signatureLibrary);

        /// <summary>
        /// Получить строки согласования с ответственным лицом без подписи
        /// </summary>
        public abstract IResultAppCollection<IStampApproval> GetStampApprovalRows();

        /// <summary>
        /// Получить строки согласования без подписи Word для извещения с изменениями
        /// </summary>
        public abstract IResultAppCollection<IStampApprovalChange> GetStampApprovalChangeRows();

        /// <summary>
        /// Получить строки согласования без подписи Word для извещения с изменениями
        /// </summary>
        public abstract IResultAppCollection<IStampApprovalPerformers> GetStampApprovalPerformersRows();
    }
}