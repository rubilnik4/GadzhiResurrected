using GadzhiMicrostation.Extensions.Microstation;
using GadzhiMicrostation.Microstation.Implementations.Units;
using GadzhiMicrostation.Microstation.Interfaces;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Models.Enum;
using MicroStationDGN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Microstation.Implementations.Elements
{
    public abstract class ElementMicrostation : IElementMicrostation
    {
        /// <summary>
        /// Экземпляр элемента Microstation
        /// </summary>
        private readonly Element _element;

        /// <summary>
        /// Родительский элемент
        /// </summary>
        private readonly IOwnerContainer _ownerContainer;
       
        public ElementMicrostation(Element element,
                                   IOwnerContainer ownerContainer)
        {
            _element = element;
            _ownerContainer = ownerContainer;
        }

        /// <summary>
        /// Коэффициент преобразования координат в текущие относительно родительского элемента
        /// </summary>
        public virtual double UnitScale => _ownerContainer.UnitScale;

        /// <summary>
        /// Является ли базовый элемент Microstation текстовым
        /// </summary>       
        public bool IsTextElementMicrostation => this is ITextElementMicrostation;

        /// <summary>
        /// Является ли базовый элемент Microstation текстовым полем
        /// </summary>       
        public bool IsTextNodeElementMicrostation => this is ITextNodeElementMicrostation;

        /// <summary>
        /// Преобразование базового элемента Microstation в текстовый элемент
        /// </summary>       
        public ITextElementMicrostation AsTextElementMicrostation => (ITextElementMicrostation)this;

        /// <summary>
        /// Преобразование базового элемента Microstation в текстовым полем
        /// </summary>       
        public ITextNodeElementMicrostation AsTextNodeElementMicrostation => (ITextNodeElementMicrostation)this;
    }
}
