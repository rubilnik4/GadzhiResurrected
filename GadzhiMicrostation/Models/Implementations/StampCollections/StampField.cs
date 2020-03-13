using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Models.Implementations.StampCollections
{
    /// <summary>
    /// Базовая ячейка штампа
    /// </summary>
    public class StampField : IStampField
    {
        /// <summary>
        /// Элемент ячейка определяющая поле штампа
        /// </summary>
        private readonly IElementMicrostation _elementStamp;

        /// <summary>
        /// Тип поля в штампе
        /// </summary>
        public StampFieldType StampFieldType { get; }

        public StampField(IElementMicrostation elementStamp, StampFieldType stampFieldType)
        {
            _elementStamp = elementStamp;
            StampFieldType = stampFieldType;
        }       
    }
}
