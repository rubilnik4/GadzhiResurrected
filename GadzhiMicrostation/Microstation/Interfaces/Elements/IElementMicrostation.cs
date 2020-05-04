using GadzhiMicrostation.Models.Implementations.Coordinates;
using GadzhiMicrostation.Models.Enums;
using System.Collections.Generic;

namespace GadzhiMicrostation.Microstation.Interfaces.Elements
{
    /// <summary>
    /// Элемент типа Microstation
    /// </summary>
    public interface IElementMicrostation : IOwnerMicrostation
    {
        /// <summary>
        /// идентификатор элемента
        /// </summary>
        long Id { get; }

        /// <summary>
        /// Является ли базовый элемент Microstation текстовым
        /// </summary>       
        bool IsTextElementMicrostation { get; }

        /// <summary>
        /// Является ли базовый элемент Microstation текстовым полем
        /// </summary>       
        bool IsTextNodeElementMicrostation { get; }

        /// <summary>
        /// Является ли базовый элемент Microstation ячейкой
        /// </summary>       
        bool IsCellElementMicrostation { get; }

        /// <summary>
        /// Преобразование базового элемента Microstation в текстовый элемент
        /// </summary>       
        ITextElementMicrostation AsTextElementMicrostation { get; }

        /// <summary>
        /// Преобразование базового элемента Microstation в текстовое поле
        /// </summary>       
        ITextNodeElementMicrostation AsTextNodeElementMicrostation { get; }

        /// <summary>
        /// Преобразование базового элемента Microstation в ячейку
        /// </summary>       
        ICellElementMicrostation AsCellElementMicrostation { get; }

        /// <summary>
        /// Тип элемента Microstation
        /// </summary>
        ElementMicrostationType ElementType { get; }

        /// <summary>
        /// Переместить элемент
        /// </summary>
        IElementMicrostation Move(PointMicrostation origin);

        /// <summary>
        /// Повернуть элемент
        /// </summary>
        IElementMicrostation Rotate(PointMicrostation origin, double degree);

        /// <summary>
        /// Масштабировать элемент
        /// </summary>
        IElementMicrostation ScaleAll(PointMicrostation origin, PointMicrostation scaleFactor);

        /// <summary>
        /// Удалить текущий элемент
        /// </summary>
        void Remove();

        /// <summary>
        /// Получить значение атрибута по его Id номеру
        /// </summary>    
        string GetAttributeById(ElementMicrostationAttributes elementAttributes);

        /// <summary>
        /// Записать значение атрибута по его Id номеру
        /// </summary>       
        void SetAttributeById(ElementMicrostationAttributes elementAttributes, string attributeValue);

        /// <summary>
        /// Получить имя элемента из атрибутов
        /// </summary>
        string AttributeControlName { get; set; }

        /// <summary>
        /// идентификатор личности
        /// </summary>    
        string AttributePersonId { get; }
    }
}
