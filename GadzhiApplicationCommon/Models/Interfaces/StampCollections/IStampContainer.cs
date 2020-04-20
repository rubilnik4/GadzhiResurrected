using GadzhiApplicationCommon.Models.Interfaces.Errors;
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
        IResultAppCollection<IStamp> Stamps { get; }
    }
}
