﻿using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Models.Interfaces.StampCollections.StampCollections
{
    /// <summary>
    /// Базовая ячейка штампа Microstation
    /// </summary>  
    public interface IStampFieldMicrostation<TElement> : IStampField
                                                         where TElement : class, IElementMicrostation
    {
        /// <summary>
        /// Текстовый элемент, определяющий поле штампа
        /// </summary>
        TElement ElementStamp { get; }
    }
}
