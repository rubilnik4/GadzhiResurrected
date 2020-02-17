using GadzhiMicrostation.Microstation.Interfaces;
using MicroStationDGN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Microstation.Implementations
{
    public class ElementMicrostation : IElementMicrostation
    {
        /// <summary>
        /// Экземпляр элемента Microstatioт
        /// </summary>
        private readonly Element _element;

        public ElementMicrostation(Element element)
        {
            _element = element;

        }
        

    }
}
