using GadzhiMicrostation.Models.Implementations.Coordinates;
using GadzhiMicrostation.Models.Enums;

namespace GadzhiMicrostation.Microstation.Interfaces.Elements
{
    /// <summary>
    /// Элемент типа Microstation
    /// </summary>
    public interface IElementMicrostation : IOwnerContainerMicrostation
    {
        /// <summary>
        /// Идентефикатор элемента
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
        /// Преобразование базового элемента Microstation в текстовый элемент
        /// </summary>       
        ITextElementMicrostation AsTextElementMicrostation { get; }

        /// <summary>
        /// Преобразование базового элемента Microstation в текстовое поле
        /// </summary>       
        ITextNodeElementMicrostation AsTextNodeElementMicrostation { get; }

        /// <summary>
        /// Тип элемента Microstation
        /// </summary>
        ElementMicrostationType ElementType { get; }

        /// <summary>
        /// Переместить элемент
        /// </summary>
        void Move(PointMicrostation origin);

        /// <summary>
        /// Повернуть элемент
        /// </summary>
        void Rotate(PointMicrostation origin, double degree);

        /// <summary>
        /// Масштабировать элемент
        /// </summary>
        void ScaleAll(PointMicrostation origin, PointMicrostation scaleFactor);

        /// <summary>
        /// Удалить текущий элемент
        /// </summary>
        void Remove();

        /// <summary>
        /// Получить значение аттрибута по его Id номеру
        /// </summary>    
        string GetAttributeById(ElementMicrostationAttributes elementAttributes);

        /// <summary>
        /// Записать значение аттрибута по его Id номеру
        /// </summary>       
        void SetAttributeById(ElementMicrostationAttributes elementAttributes, string attributeValue);

        /// <summary>
        /// Получить имя элемента из аттрибутов
        /// </summary>
        string AttributeControlName { get; set; }

        /// <summary>
        /// Получить идентефикатор личности
        /// </summary>    
        string AttributePersonId { get; }
    }
}
