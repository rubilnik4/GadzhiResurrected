using GadzhiMicrostation.Microstation.Implementations;
using GadzhiMicrostation.Models.Implementations.Coordinates;
using GadzhiMicrostation.Models.Enums;
using MicroStationDGN;
using GadzhiApplicationCommon.Models.Interfaces.Errors;

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
        /// Записать значение аттрибута по его Id номеру
        /// </summary>       
        public static void SetAttributeById(this Element element, ElementMicrostationAttributes elementAttribute, string attributeValue) =>
           AttributesElementsMicrostation.SetAttributeById(element, elementAttribute, attributeValue);

        /// <summary>
        /// Получить имя элемента из аттрибутов
        /// </summary>
        public static string GetAttributeControlName(this Element element) =>
             AttributesElementsMicrostation.GetAttributeControlName(element);

        /// <summary>
        /// Записать имя элемента из аттрибутов
        /// </summary>
        public static void SetAttributeControlName(this Element element, string controlName) =>
             AttributesElementsMicrostation.SetAttributeControlName(element, controlName);

        /// <summary>
        /// Получить размеры ячейки элемента в стандартных координатах
        /// </summary>
        public static IResultAppValue<RangeMicrostation> GetAttributeRange(this Element element) =>
             AttributesElementsMicrostation.GetAttributeRange(element);

        /// <summary>
        /// Получить идентефикатор личности
        /// </summary>
        public static string GetAttributePersonId(this Element element) =>
              AttributesElementsMicrostation.GetAttributePersonId(element);
    }
}
