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

        /// <summary>
        /// Класс для работы с приложением Microstation
        /// </summary>
        public IApplicationMicrostation ApplicationMicrostation { get; }

        public ElementMicrostation(Element element,
                                   IOwnerContainer ownerContainer)
        {
            _element = element;
            _ownerContainer = ownerContainer;
            ApplicationMicrostation = _ownerContainer.ApplicationMicrostation;
        }

        /// <summary>
        /// Идентефикатор элемента
        /// </summary>
        public long Id => _element.ID64;

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

        /// <summary>
        /// Тип элемента Microstation
        /// </summary>
        public ElementMicrostationType ElementType
        {
            get
            {
                ElementMicrostationType elementType = ElementMicrostationType.Element;

                if (IsTextElementMicrostation)
                {
                    elementType = ElementMicrostationType.TextElement;
                }
                else if (IsTextNodeElementMicrostation)
                {
                    elementType = ElementMicrostationType.TextNodeElement;
                }

                return elementType;
            }
        }

        /// <summary>
        /// Получить значение аттрибута по его Id номеру
        /// </summary>       
        public string GetAttributeById(ElementMicrostationAttributes elementAttributes) =>
            _element.GetAttributeById(elementAttributes);

        /// <summary>
        /// Получить идентефикатор личности
        /// </summary>    
        public string GetAttributePersonId() => _element.GetAttributePersonId();
    }
}
