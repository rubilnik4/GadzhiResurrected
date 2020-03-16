using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Models.Interfaces.StampCollections.StampCollections
{
    /// <summary>
    /// Штамп. Базовый вариант Microstation
    /// </summary>
    public interface IStampMicrostation: IStamp
    {
        /// <summary>
        /// Элемент ячейка, определяющая штамп
        /// </summary>
        ICellElementMicrostation StampCellElement { get; }
    }
}
