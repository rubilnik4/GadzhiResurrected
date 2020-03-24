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
        TField Signature { get; }

        /// <summary>
        /// Установлена ли подпись
        /// </summary>
        bool IsSignatureValid { get; }

        /// <summary>
        /// Идентефикатор личности
        /// </summary>    
        string AttributePersonId { get; }

        /// <summary>
        /// Ответственное лицо
        /// </summary>    
        string PersonName { get; }

        /// <summary>
        /// Вставить подпись
        /// </summary>
        void InsertSignature();

        /// <summary>
        /// Удалить текущую подпись
        /// </summary>
        void DeleteSignature();
    }
}
