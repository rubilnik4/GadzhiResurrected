using System.Collections.Generic;
using System.Linq;
using GadzhiApplicationCommon.Extensions.Functional.Result;
using GadzhiApplicationCommon.Models.Implementation.StampCollections.Fields;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.LibraryData;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Fields;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Signatures;

namespace GadzhiApplicationCommon.Models.Implementation.StampCollections.StampPartial
{
    /// <summary>
    /// Подкласс штампа для работы с подписями
    /// </summary>
    public abstract partial class Stamp
    {
        /// <summary>
        /// Вставить подписи
        /// </summary>
        public abstract IResultAppCollection<IStampSignature> InsertSignatures();

        /// <summary>
        /// Удалить подписи
        /// </summary>
        public abstract IResultAppCollection<IStampSignature> DeleteSignatures(IEnumerable<IStampSignature> signatures);

        /// <summary>
        /// Получить поля штампа, отвечающие за подписи
        /// </summary>
        protected abstract IStampSignatureFields GetStampSignatureFields();

        /// <summary>
        /// Получить строки с ответственным лицом без подписи
        /// </summary>
        protected abstract IResultAppCollection<IStampPerson> GetStampPersonRows();

        /// <summary>
        /// Получить строки с изменениями
        /// </summary>
        protected abstract IResultAppCollection<IStampChange> GetStampChangeRows(ISignatureLibraryApp signatureLibrary);

        /// <summary>
        /// Получить строки согласования с ответственным лицом без подписи
        /// </summary>
        protected abstract IResultAppCollection<IStampApproval> GetStampApprovalRows();

        /// <summary>
        /// Получить строки с согласованием без подписи Word для извещения с изменениями
        /// </summary>
        protected abstract IResultAppCollection<IStampApprovalChange> GetStampApprovalChangeRows();
    }
}