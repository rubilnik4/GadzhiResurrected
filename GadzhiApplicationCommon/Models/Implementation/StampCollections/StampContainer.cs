using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiApplicationCommon.Models.Implementation.StampCollections
{
    /// <summary>
    /// Контейнер штампов. Базовый класс
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
    }
}
