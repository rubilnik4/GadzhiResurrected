using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GadzhiApplicationCommon.Models.Interfaces.LibraryData;
using GadzhiApplicationCommon.Models.Implementation.LibraryData;

namespace GadzhiApplicationCommon.Models.Implementation.StampCollections
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
        public abstract PersonInformation PersonInformation { get; }

        /// <summary>
        /// Корректно ли заполнено поле ответственного лица
        /// </summary>
        public virtual bool IsPersonFieldValid() => !String.IsNullOrEmpty(PersonId);

        /// <summary>
        /// Вставить подпись
        /// </summary>
        public abstract IStampSignature<TField> InsertSignature(ISignatureFile signatureFile);

        /// <summary>
        /// Удалить подпись
        /// </summary>
        public abstract IStampSignature<TField> DeleteSignature();
    }
}
