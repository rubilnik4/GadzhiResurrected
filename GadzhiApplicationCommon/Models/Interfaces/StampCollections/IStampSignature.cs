using GadzhiApplicationCommon.Models.Interfaces.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GadzhiApplicationCommon.Models.Interfaces.LibraryData;

namespace GadzhiApplicationCommon.Models.Interfaces.StampCollections
{
    /// <summary>
    /// Базовая структура подписи
    /// </summary>
    public interface IStampSignature<out TField> : ISignatureLibrary
        where TField : IStampField
    {
        /// <summary>
        /// Подпись
        /// </summary>
        IResultAppValue<TField> Signature { get; }

        /// <summary>
        /// Установлена ли подпись
        /// </summary>
        bool IsSignatureValid();

        /// <summary>
        /// Корректно ли заполнено поле ответственного лица
        /// </summary>
        bool IsPersonFieldValid();

        /// <summary>
        /// Вставить подпись
        /// </summary>
        IStampSignature<TField> InsertSignature(ISignatureFile signatureFile);

        /// <summary>
        /// Удалить текущую подпись
        /// </summary>
        IStampSignature<TField> DeleteSignature();
    }
}
