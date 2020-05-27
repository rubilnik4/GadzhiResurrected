using System;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.LibraryData;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Fields;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Signatures;

namespace GadzhiApplicationCommon.Models.Implementation.StampCollections.Signatures
{
    /// <summary>
    /// Базовая структура подписи
    /// </summary>
    public abstract class StampSignature : IStampSignature
    {
        protected StampSignature(ISignatureLibraryApp signatureLibrary)
        {
            SignatureLibrary = signatureLibrary ?? throw new ArgumentNullException(nameof(signatureLibrary));
        }

        /// <summary>
        /// Имя с идентификатором
        /// </summary>    
        public ISignatureLibraryApp SignatureLibrary { get; }

        /// <summary>
        /// Подпись
        /// </summary>
        public abstract IResultAppValue<IStampField> Signature { get; }

        /// <summary>
        /// Установлена ли подпись
        /// </summary>
        public abstract bool IsSignatureValid();

        /// <summary>
        /// Корректно ли заполнено поле ответственного лица
        /// </summary>
        public bool IsPersonFieldValid() => !String.IsNullOrEmpty(SignatureLibrary.PersonId);

        /// <summary>
        /// Необходимо ли вставлять подпись в поле
        /// </summary>
        public virtual bool NeedToInsert() => IsPersonFieldValid();

        /// <summary>
        /// Вставить подпись
        /// </summary>
        public abstract IStampSignature InsertSignature(ISignatureFileApp signatureFile);

        /// <summary>
        /// Удалить подпись
        /// </summary>
        public abstract IStampSignature DeleteSignature();
    }
}
