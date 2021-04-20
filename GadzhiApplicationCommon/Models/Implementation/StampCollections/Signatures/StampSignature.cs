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
        protected StampSignature(ISignatureLibraryApp signatureLibrary, StampIdentifier stampIdentifier)
        {
            SignatureLibrary = signatureLibrary ?? throw new ArgumentNullException(nameof(signatureLibrary));
            StampIdentifier = stampIdentifier;
        }

        /// <summary>
        /// Имя с идентификатором
        /// </summary>    
        public ISignatureLibraryApp SignatureLibrary { get; }

        /// <summary>
        /// Идентификатор штампа
        /// </summary>
        public StampIdentifier StampIdentifier { get; }

        /// <summary>
        /// Подпись
        /// </summary>
        public abstract IResultAppValue<IStampField> Signature { get; }

        /// <summary>
        /// Вертикальное расположение подписи
        /// </summary>
        public bool IsVertical => Signature.Value?.IsVertical ?? false;

        /// <summary>
        /// Установлена ли подпись
        /// </summary>
        public abstract bool IsSignatureValid { get; }

        /// <summary>
        /// Корректно ли заполнено поле ответственного лица
        /// </summary>
        public bool IsPersonFieldValid => !String.IsNullOrEmpty(SignatureLibrary.PersonId);

        /// <summary>
        /// Необходимо ли вставлять подпись в поле
        /// </summary>
        public virtual bool IsAbleToInsert => IsPersonFieldValid;

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
