using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.LibraryData;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Signatures;

namespace GadzhiApplicationCommon.Models.Interfaces.StampCollections.StampPartial.SignatureCreating
{
    /// <summary>
    /// Фабрика создания подписей Word
    /// </summary>
    public interface ISignatureCreating
    {
        /// <summary>
        /// Получить строки с ответственным лицом без подписи
        /// </summary>
        IResultAppCollection<IStampPerson> GetStampPersonRows();

        /// <summary>
        /// Получить строки с изменениями
        /// </summary>
        IResultAppCollection<IStampChange> GetStampChangeRows(ISignatureLibraryApp signatureLibrary);

        /// <summary>
        /// Получить строки согласования с ответственным лицом без подписи
        /// </summary>
        IResultAppCollection<IStampApproval> GetStampApprovalRows();

        /// <summary>
        /// Получить строки согласования без подписи Word для извещения с изменениями
        /// </summary>
        IResultAppCollection<IStampApprovalChange> GetStampApprovalChangeRows();

        /// <summary>
        /// Получить строки согласования без подписи Word для извещения с изменениями
        /// </summary>
        IResultAppCollection<IStampApprovalPerformers> GetStampApprovalPerformersRows();
    }
}