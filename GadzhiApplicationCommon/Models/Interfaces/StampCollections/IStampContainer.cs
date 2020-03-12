using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiApplicationCommon.Models.Interfaces.StampCollections
{
    /// <summary>
    /// Контейнер штампов
    /// </summary>
    public interface IStampContainer
    {
        /// <summary>
        /// Список штампов
        /// </summary>
        IEnumerable<IStamp> Stamps { get; }
        
        /// <summary>
        /// Получить список всех строк с подписями во всех штампах
        /// </summary>     
        IEnumerable<IStampPersonSignature> GetStampPersonSignatures();

        /// <summary>
        /// Корретность загрузки штампов
        /// </summary>
        bool IsValid { get; }
    }
}
