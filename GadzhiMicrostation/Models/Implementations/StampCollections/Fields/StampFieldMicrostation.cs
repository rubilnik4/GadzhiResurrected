﻿using System;
using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiApplicationCommon.Models.Implementation.StampCollections.Fields;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Models.Interfaces.StampCollections;
using GadzhiMicrostation.Models.Interfaces.StampCollections.Fields;

namespace GadzhiMicrostation.Models.Implementations.StampCollections.Fields
{
    /// <summary>
    /// Ячейка штампа Microstation
    /// </summary>
    public class StampFieldMicrostation : StampField, IStampFieldMicrostation
    {
        public StampFieldMicrostation(IElementMicrostation elementStamp, StampFieldType stampFieldType)
            : base(stampFieldType)
        {
            ElementStamp = elementStamp ?? throw new ArgumentNullException(nameof(elementStamp));
        }

        /// <summary>
        /// Элемент, определяющий поле штампа
        /// </summary>
        public IElementMicrostation ElementStamp { get; }
    }
}
