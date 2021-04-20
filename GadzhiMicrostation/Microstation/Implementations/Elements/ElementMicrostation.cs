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
using System.ComponentModel;
using System.Drawing;

namespace GadzhiMicrostation.Microstation.Implementations.Elements
{
    public abstract class ElementMicrostation : IElementMicrostation
    {
        protected ElementMicrostation(Element element, IOwnerMicrostation ownerContainerMicrostation)
        {
            _element = element ?? throw new ArgumentNullException(nameof(element));
            OwnerContainerMicrostation = ownerContainerMicrostation ?? throw new ArgumentNullException(nameof(ownerContainerMicrostation));
            ApplicationMicrostation = OwnerContainerMicrostation.ApplicationMicrostation;
            ModelMicrostation = OwnerContainerMicrostation.ModelMicrostation;
        }


        /// <summary>
        /// Экземпляр элемента Microstation
        /// </summary>
        private readonly Element _element;

        /// <summary>
        /// Родительский элемент
        /// </summary>
        protected IOwnerMicrostation OwnerContainerMicrostation { get; }

        /// <summary>
        /// Класс для работы с приложением Microstation
        /// </summary>
        public IApplicationMicrostation ApplicationMicrostation { get; }

        /// <summary>
        /// Модель или лист в файле
        /// </summary>
        public IModelMicrostation ModelMicrostation { get; }

        /// <summary>
        /// Идентификатор элемента
        /// </summary>
        private long? _id;

        /// <summary>
        /// Идентификатор элемента
        /// </summary>
        public long Id => _id ??= _element.ID64;

        /// <summary>
        /// Коэффициент преобразования координат в текущие относительно родительского элемента
        /// </summary>
        public virtual double UnitScale => OwnerContainerMicrostation.UnitScale;

        /// <summary>
        /// Является ли базовый элемент Microstation линией
        /// </summary>       
        public bool IsLineElementMicrostation => this is ILineElementMicrostation;

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
        /// Преобразование базового элемента Microstation в линию
        /// </summary>       
        public ILineElementMicrostation AsLineElementMicrostation => (ILineElementMicrostation)this;

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
        public ElementMicrostationType ElementType =>
            this switch
            {
                _ when IsLineElementMicrostation => ElementMicrostationType.LineElement,
                _ when IsTextElementMicrostation => ElementMicrostationType.TextElement,
                _ when IsTextNodeElementMicrostation => ElementMicrostationType.TextNodeElement,
                _ when IsCellElementMicrostation => ElementMicrostationType.CellElement,
                _ => throw new InvalidOperationException("Element type not found"),
            };

        /// <summary>
        /// Вес линий элемента
        /// </summary>
        private int? _lineWeight;

        /// <summary>
        /// Вес линий элемента
        /// </summary>
        public int LineWeight
        {
            get => _lineWeight ??= _element.LineWeight;
            set
            {
                int lineWeight = (value >= 0) ? value : 0;
                _lineWeight = lineWeight;
                _element.LineWeight = lineWeight;
            }
        }

        /// <summary>
        /// Переместить элемент
        /// </summary>
        protected virtual TElement Move<TElement>(PointMicrostation offset)
            where TElement : IElementMicrostation
        {
            _element.Move(offset.ToPoint3d());
            return Clone<TElement>();
        }

        /// <summary>
        /// Повернуть элемент
        /// </summary>
        protected virtual TElement Rotate<TElement>(PointMicrostation origin, double degree)
            where TElement : IElementMicrostation
        {
            _element.AsCellElement.RotateAboutZ(origin.ToPoint3d(), degree * (Math.PI / 180));
            return Clone<TElement>();
        }

        /// <summary>
        /// Масштабировать элемент
        /// </summary>
        protected virtual TElement ScaleAll<TElement>(PointMicrostation origin, PointMicrostation scaleFactor)
            where TElement : IElementMicrostation
        {
            _element.ScaleAll(origin.ToPoint3d(), scaleFactor.X, scaleFactor.Y, scaleFactor.Z);
            return Clone<TElement>();
        }

        /// <summary>
        /// Копировать элемент
        /// </summary>
        protected abstract TElement Clone<TElement>()
            where TElement : IElementMicrostation;

        /// <summary>
        /// Удалить текущий элемент
        /// </summary>
        public void Remove()
        {
            var parentId = _element.ParentID;
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
        /// Доступность элемента 
        /// </summary>
        public bool IsValid()
        {
            try
            {
                return _element.IsValid;
            }
            catch
            {
                return false;
            }
        }
          

        /// <summary>
        /// Кэшированные атрибуты элемента
        /// </summary>
        protected IDictionary<ElementMicrostationAttributes, string> AttributeCaching { get; } =
            new Dictionary<ElementMicrostationAttributes, string>();

        /// <summary>
        /// Имя элемента из атрибутов
        /// </summary>
        public string AttributeControlName
        {
            get => GetAttributeFromCacheOrLoad(ElementMicrostationAttributes.ControlName);
            set => SetAttributeToCacheAndUpload(ElementMicrostationAttributes.ControlName, value);
        }

        /// <summary>
        /// идентификатор личности
        /// </summary>    
        public string AttributePersonId => 
            GetAttributeFromCacheOrLoad(ElementMicrostationAttributes.PersonId);

        /// <summary>
        /// Получить значение атрибута по его PersonId номеру
        /// </summary>       
        public string GetAttributeById(ElementMicrostationAttributes elementAttribute) =>
            GetAttributeFromCacheOrLoad(elementAttribute);

        /// <summary>
        /// Записать значение атрибута по его PersonId номеру
        /// </summary>       
        public void SetAttributeById(ElementMicrostationAttributes elementAttribute, string attributeValue) =>
            SetAttributeToCacheAndUpload(elementAttribute, attributeValue);

        /// <summary>
        /// Построить изображение согласно размерам
        /// </summary>
        public void DrawToEmfFile(string filepath, int width, int height) => _element.DrawToFile(filepath, width, height);

        /// <summary>
        /// Получить атрибут из кэша или выгрузить из Microstation
        /// </summary>       
        protected string GetAttributeFromCacheOrLoad(ElementMicrostationAttributes attributeElement)
        {
            if (AttributeCaching.ContainsKey(attributeElement)) return AttributeCaching[attributeElement];

            string attribute = _element.GetAttributeById(attributeElement);
            AttributeCaching[attributeElement] = attribute;
            return attribute;
        }

        /// <summary>
        /// Добавить атрибут в кэш
        /// </summary>        
        protected void SetAttributeToCacheAndUpload(ElementMicrostationAttributes elementAttribute, string attributeValue)
        {
            _element.SetAttributeById(elementAttribute, attributeValue);
            AttributeCaching[elementAttribute] = attributeValue;
        }
    }
}
