using GadzhiApplicationCommon.Models.Interfaces.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiApplicationCommon.Models.Interfaces.StampCollections
{
    /// <summary>
    /// Базовая структура подписи
    /// </summary>
    public interface IStampSignature<out TField> where TField : IStampField
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
        /// Идентификатор личности
        /// </summary>    
        string PersonId { get; }

        /// <summary>
        /// Ответственное лицо
        /// </summary>    
        string PersonName { get; }

        /// <summary>
        /// Корректно ли заполнено поле ответственного лица
        /// </summary>
        bool IsPersonFieldValid();

        /// <summary>
        /// Вставить подпись
        /// </summary>
        IStampSignature<TField> InsertSignature();

        /// <summary>
        /// Удалить текущую подпись
        /// </summary>
        IStampSignature<TField> DeleteSignature();
    }
}
