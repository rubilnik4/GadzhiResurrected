using GadzhiMicrostation.Microstation.Interfaces.Elements;
using MicroStationDGN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Microstation.Implementations.Elements
{
    /// <summary>
    /// Связка элементов в библиотечной форме и текущей обертке
    /// </summary>
    public class ElementMicrostationPair
    {
        /// <summary>
        /// Элемент в текущей обертке
        /// </summary>
        public IElementMicrostation ElementWrapper { get; }

        /// <summary>
        /// Элемент в текущей обертке
        /// </summary>
        public Element ElementOriginal { get; }

        public ElementMicrostationPair(IElementMicrostation elementWrapper, Element elementOriginal)
        {
            ElementWrapper = elementWrapper;
            ElementOriginal = elementOriginal;
        }
    }
}
