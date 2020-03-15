using GadzhiApplicationCommon.Models.Enums;
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
        public ITextElementMicrostation TextElementStamp { get; }

        public StampFieldMicrostation(ITextElementMicrostation textElementStamp, StampFieldType stampFieldType)
            : base(stampFieldType)
        {
            TextElementStamp = textElementStamp ?? 
                               throw new ArgumentNullException(nameof(textElementStamp));
        }

        /// <summary>
        /// Текст поля в штампе
        /// </summary>
        public override string Text => TextElementStamp.Text;
    }
}
