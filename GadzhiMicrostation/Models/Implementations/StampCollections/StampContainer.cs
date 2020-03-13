using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Models.Implementations.StampCollections
{
    /// <summary>
    /// Контейнер штампов
    /// </summary>
    public class StampContainer : IStampContainer
    {
        /// <summary>
        /// Список штампов
        /// </summary>
        public IEnumerable<IStamp> Stamps { get; }

        public StampContainer(IEnumerable<IStamp> stamps)
        {
            Stamps = stamps;
        }

        /// <summary>
        /// Корретность загрузки штампов
        /// </summary>
        public bool IsValid => Stamps != null;

        /// <summary>
        /// Получить список всех строк с подписями во всех штампах
        /// </summary>     
        public IEnumerable<IStampPersonSignature> GetStampPersonSignatures() => Stamps?.OfType<IStampMain>()?.
                                                                                SelectMany(stamp => stamp.StampPersonSignatures);

        
    }
}
