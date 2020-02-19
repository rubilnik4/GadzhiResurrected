using GadzhiMicrostation.Microstation.Implementations;
using GadzhiMicrostation.Models.Coordinates;
using GadzhiMicrostation.Models.Enum;
using GadzhiMicrostation.Models.StampCollections;
using MicroStationDGN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Extensions.Microstation
{
    /// <summary>
    /// Методы расширения для ячеек Microstation
    /// </summary>
    public static class ElementAttributeExtensions
    {
        /// <summary>
        /// Получить значение аттрибута по его Id номеру
        /// </summary>       
        public static string GetAttributeById(this Element element, ElementMicrostationAttributes elementAttribute) =>
           AttributesElementsMicrostation.GetAttributeById(element, elementAttribute);

        /// <summary>
        /// Получить имя элемента из аттрибутов
        /// </summary>
        public static string GetAttributeControlName(this Element element) =>
             AttributesElementsMicrostation.GetAttributeById(element, ElementMicrostationAttributes.ControlName);

        /// <summary>
        /// Получить размеры ячейки элемента в стандартных координатах
        /// </summary>
        public static RangeMicrostation GetAttributeRange(this Element element, bool isVertical) =>
             AttributesElementsMicrostation.GetAttributeRange(element, isVertical);

        /// <summary>
        /// Получить идентефикатор личности
        /// </summary>
        public static string GetAttributePersonId(this Element element) =>
              AttributesElementsMicrostation.GetAttributePersonId(element);
    }
}
