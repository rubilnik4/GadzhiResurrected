using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiApplicationCommon.Models.Interfaces.StampCollections
{
    /// <summary>
    /// Основные поля штампа
    /// </summary>
    public interface IStampMain<TField> : IStamp 
                                          where TField : IStampField
    {
        /// <summary>
        /// Строки с ответсвенным лицом и подписью
        /// </summary>
        IEnumerable<IStampPersonSignature<TField>> StampPersonSignatures { get; }
    }
}
