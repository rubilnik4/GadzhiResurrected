using GadzhiMicrostation.Extensions.Microstation;
using GadzhiMicrostation.Microstation.Interfaces;
using GadzhiMicrostation.Microstation.Interfaces.ApplicationMicrostationPartial;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Models.Implementations.Coordinates;
using GadzhiMicrostation.Models.Enums;
using MicroStationDGN;
using System;
using System.Collections;
using System.Collections.Generic;

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
        protected IOwnerMicrostation OwnerContainerMicrostation { get; private set; }

        /// <summary>
        /// Класс для работы с приложением Microstation
        /// </summary>
        public IApplicationMicrostation ApplicationMicrostation { get; }

        /// <summary>
        /// Модель или лист в файле
        /// </summary>
        public IModelMicrostation ModelMicrostation { get; }

        public ElementMicrostation(Element element,
                                   IOwnerMicrostation ownerContainerMicrostation)
        {
            _element = element ?? throw new ArgumentNullException(nameof(element));
            OwnerContainerMicrostation = ownerContainerMicrostation;
            ApplicationMicrostation = OwnerContainerMicrostation?.ApplicationMicrostation;
            ModelMicrostation = OwnerContainerMicrostation.ModelMicrostation;

            AttributeCaching = new Dictionary<ElementMicrostationAttributes, string>();
        }

        /// <summary>
        /// Идентефикатор элемента
        /// </summary>
        public long Id => _element.ID64;

        /// <summary>
        /// Коэффициент преобразования координат в текущие относительно родительского элемента
        /// </summary>
        public virtual double UnitScale => OwnerContainerMicrostation.UnitScale;

        /// <summary>
        /// Является ли базовый элемент Microstation текстовым
        /// </summary>       
        public bool IsTextElementMicrostation => this is ITextElementMicrostation;

        /// <summary>
        /// Является ли базовый элемент Microstation текстовым полем
        /// </summary>       
        public bool IsTextNodeElementMicrostation => this is ITextNodeElementMicrostation;

        /// <summary>
        /// Является ли базовый элемент Microstation ячейкой
        /// </summary>       
        public bool IsCellElementMicrostation => this is ICellElementMicrostation;

        /// <summary>
        /// Преобразование базового элемента Microstation в текстовый элемент
        /// </summary>       
        public ITextElementMicrostation AsTextElementMicrostation => (ITextElementMicrostation)this;

        /// <summary>
        /// Преобразование базового элемента Microstation в текстовым полем
        /// </summary>       
        public ITextNodeElementMicrostation AsTextNodeElementMicrostation => (ITextNodeElementMicrostation)this;

        /// <summary>
        /// Преобразование базового элемента Microstation в ячейку
        /// </summary>       
        public ICellElementMicrostation AsCellElementMicrostation => (ICellElementMicrostation)this;

        /// <summary>
        /// Тип элемента Microstation
        /// </summary>
        public ElementMicrostationType ElementType
        {
            get
            {                
                if (IsTextElementMicrostation)
                {
                    return ElementMicrostationType.TextElement;
                }
                else if (IsTextNodeElementMicrostation)
                {
                    return ElementMicrostationType.TextNodeElement;
                }
                else if (IsCellElementMicrostation)
                {
                    return ElementMicrostationType.CellElement;
                }
                else
                {
                    return ElementMicrostationType.Element;
                }
            }
        }

        /// <summary>
        /// Переместить элемент
        /// </summary>
        public virtual IElementMicrostation Move(PointMicrostation offset)
        {
            _element.Move(offset.ToPoint3d());
            return this;
        }

        /// <summary>
        /// Повернуть элемент
        /// </summary>
        public virtual IElementMicrostation Rotate(PointMicrostation origin, double degree)
        {
            _element.RotateAboutZ(origin.ToPoint3d(), degree * (Math.PI / 180));
            return this;
        }

        /// <summary>
        /// Масштабировать элемент
        /// </summary>
        public virtual IElementMicrostation ScaleAll(PointMicrostation origin, PointMicrostation scaleFactor)
        {
            _element.ScaleAll(origin.ToPoint3d(), scaleFactor.X, scaleFactor.Y, scaleFactor.Z);
            return this;
        }

        /// <summary>
        /// Удалить текущий элемент
        /// </summary>
        public void Remove()
        {
            DLong parentId = _element.ParentID;
            if (parentId.High == 0 && parentId.Low == 0)
            {
                ModelMicrostation.RemoveElement(Id);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Кэшированные аттрибуты элемента
        /// </summary>
        protected IDictionary<ElementMicrostationAttributes, string> AttributeCaching { get; }

        /// <summary>
        /// Получить значение аттрибута по его Id номеру
        /// </summary>       
        public string GetAttributeById(ElementMicrostationAttributes elementAttribute) =>
            GetAttributeFromCachOrLoad(elementAttribute);

        /// <summary>
        /// Записать значение аттрибута по его Id номеру
        /// </summary>       
        public void SetAttributeById(ElementMicrostationAttributes elementAttribute, string attributeValue) =>
            SetAttributeToCachAndUpload(elementAttribute, attributeValue);

        /// <summary>
        /// Имя элемента из аттрибутов
        /// </summary>
        public string AttributeControlName
        {
            get => GetAttributeFromCachOrLoad(ElementMicrostationAttributes.ControlName);
            set => SetAttributeToCachAndUpload(ElementMicrostationAttributes.ControlName, value);
        }

        /// <summary>
        /// Идентефикатор личности
        /// </summary>    
        public string AttributePersonId => GetAttributeFromCachOrLoad(ElementMicrostationAttributes.PersonId);

        /// <summary>
        /// Получить аттрибут из кэша или выгрузить из Microstation
        /// </summary>       
        protected string GetAttributeFromCachOrLoad(ElementMicrostationAttributes attribute) =>
            (AttributeCaching.ContainsKey(attribute)) ?
             AttributeCaching[attribute] :
             _element.GetAttributeById(attribute);

        /// <summary>
        /// Добавить аттрибут в кэш
        /// </summary>        
        protected void SetAttributeToCachAndUpload(ElementMicrostationAttributes elementAttribute, string attributeValue)
        {
            _element.SetAttributeById(elementAttribute, attributeValue);
            AttributeCaching[elementAttribute] = attributeValue;
        }
    }
}
