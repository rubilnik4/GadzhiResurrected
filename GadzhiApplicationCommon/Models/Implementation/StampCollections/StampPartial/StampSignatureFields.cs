using System.Collections.Generic;
using System.Linq;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Fields;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Signatures;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.StampPartial.SignatureCreating;

namespace GadzhiApplicationCommon.Models.Implementation.StampCollections.StampPartial
{
    /// <summary>
    /// Подкласс штампа для работы с подписями
    /// </summary>
    public abstract partial class Stamp
    {
        /// <summary>
        /// Фабрика создания подписей Word
        /// </summary>
        protected abstract ISignatureCreating SignatureCreating { get; }

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
    }
}