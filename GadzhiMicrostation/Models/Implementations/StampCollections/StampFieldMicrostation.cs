﻿using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Implementation.StampCollections;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Models.Interfaces.StampCollections.StampCollections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Models.Implementations.StampCollections
{
    /// <summary>
    /// Ячейка штампа Microstation
    /// </summary>
    public class StampFieldMicrostation : StampField, IStampFieldMicrostation
    {
        /// <summary>
        /// Текстовый элемент, определяющий поле штампа
        /// </summary>
        public IElementMicrostation ElementStamp { get; }

        public StampFieldMicrostation(IElementMicrostation elementStamp, StampFieldType stampFieldType)
            : base(stampFieldType)
        {
            ElementStamp = elementStamp ?? 
                           throw new ArgumentNullException(nameof(elementStamp));
        }
    }
}