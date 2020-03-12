using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiApplicationCommon.Models.Interfaces.StampCollections
{
    /// <summary>
    /// Основные поля штампа
    /// </summary>
    public interface IStampMain : IStamp
    {
        /// <summary>
        /// Строки с ответсвенным лицом и подписью
        /// </summary>
        IEnumerable<IStampPersonSignature> StampPersonSignatures { get; }
    }
}
