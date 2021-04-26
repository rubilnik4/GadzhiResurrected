using GadzhiMicrostation.Models.Implementations.Coordinates;
using GadzhiMicrostation.Models.Enums;
using System.Collections.Generic;
using System.Drawing;
using MicroStationDGN;

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
        /// Является ли базовый элемент Microstation линией
        /// </summary>       
        bool IsLineElementMicrostation { get; }

        /// <summary>
        /// Является ли базовый элемент Microstation текстовым полем
        /// </summary>       
        bool IsTextNodeElementMicrostation { get; }

        /// <summary>
        /// Является ли базовый элемент Microstation ячейкой
        /// </summary>       
        bool IsCellElementMicrostation { get; }

        /// <summary>
        /// Преобразование базового элемента Microstation в линию
        /// </summary>       
        ILineElementMicrostation AsLineElementMicrostation { get; }

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
        /// Вес линий элемента
        /// </summary>
        int LineWeight { get; set; }

        /// <summary>
        /// Получить имя элемента из атрибутов
        /// </summary>
        string AttributeControlName { get; set; }

        /// <summary>
        /// идентификатор личности
        /// </summary>    
        string AttributePersonId { get; }

        /// <summary>
        /// Удалить текущий элемент
        /// </summary>
        void Remove();

        /// <summary>
        /// Доступность элемента 
        /// </summary>
        bool IsValid();

        /// <summary>
        /// Получить значение атрибута по его PersonId номеру
        /// </summary>    
        string GetAttributeById(ElementMicrostationAttributes elementAttributes);

        /// <summary>
        /// Установить элемент
        /// </summary>
        void SetElement(Element element);

        /// <summary>
        /// Записать значение атрибута по его PersonId номеру
        /// </summary>       
        void SetAttributeById(ElementMicrostationAttributes elementAttributes, string attributeValue);

        /// <summary>
        /// Построить изображение согласно размерам
        /// </summary>
        void DrawToEmfFile(string filePath, int width, int height);
    }
}
