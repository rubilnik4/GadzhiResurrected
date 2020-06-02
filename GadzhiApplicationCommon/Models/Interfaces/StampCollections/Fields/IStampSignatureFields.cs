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
        /// Строки с изменениями
        /// </summary>
        IResultAppCollection<IStampChange> StampChanges { get; }

        /// <summary>
        /// Строки с согласованием
        /// </summary>
        IResultAppCollection<IStampApproval> StampApproval { get; }

        /// <summary>
        /// Получить все подписи
        /// </summary>        
        IResultAppCollection<IStampSignature> GetSignatures();
    }
}