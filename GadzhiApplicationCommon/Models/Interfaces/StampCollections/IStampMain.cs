using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiApplicationCommon.Models.Interfaces.StampCollections
{
    /// <summary>
    /// Основные поля штампа
    /// </summary>
    public interface IStampMain<TField, TFieldSignature> : IStamp 
                                                           where TField : IStampField
                                                           where TFieldSignature : IStampField
    {
        /// <summary>
        /// Строки с ответсвенным лицом и подписью
        /// </summary>
        IEnumerable<IStampPersonSignature<TField, TFieldSignature>> StampPersonSignatures { get; }
    }
}
