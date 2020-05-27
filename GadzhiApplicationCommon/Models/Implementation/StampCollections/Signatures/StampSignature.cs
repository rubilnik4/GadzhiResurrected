using System;
using GadzhiApplicationCommon.Models.Implementation.LibraryData;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.LibraryData;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Fields;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.Signatures;

namespace GadzhiApplicationCommon.Models.Implementation.StampCollections.Signatures
{
    /// <summary>
    /// Базовая структура подписи
    /// </summary>
    public abstract class StampSignature<TField> : IStampSignature<TField>
        where TField : IStampField
    {
        /// <summary>
        /// Подпись
        /// </summary>
        public abstract IResultAppValue<TField> Signature { get; }

        /// <summary>
        /// Установлена ли подпись
        /// </summary>
        public abstract bool IsSignatureValid();

        /// <summary>
        /// Идентификатор личности
        /// </summary>    
        public abstract string PersonId { get; }

        /// <summary>
        /// Ответственное лицо
        /// </summary>    
        public abstract PersonInformationApp PersonInformation { get; }

        /// <summary>
        /// Корректно ли заполнено поле ответственного лица
        /// </summary>
        public virtual bool IsPersonFieldValid() => !String.IsNullOrEmpty(PersonId);

        /// <summary>
        /// Вставить подпись
        /// </summary>
        public abstract IStampSignature<TField> InsertSignature(ISignatureFileApp signatureFile);

        /// <summary>
        /// Удалить подпись
        /// </summary>
        public abstract IStampSignature<TField> DeleteSignature();
    }
}
