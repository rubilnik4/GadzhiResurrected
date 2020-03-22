using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiApplicationCommon.Models.Implementation.StampCollections
{
    /// <summary>
    /// Базовая структура подписи
    /// </summary>
    public abstract class StampSignature<TField> : IStampSignature <TField>
                                                   where TField : IStampField
    {
        /// <summary>
        /// Подпись
        /// </summary>
        public abstract TField Signature { get; }

        /// <summary>
        /// Установлена ли подпись
        /// </summary>
        public abstract bool IsSignatureValid { get; }

        /// <summary>
        /// Идентефикатор личности
        /// </summary>    
        public abstract string AttributePersonId { get; }
    }
}
