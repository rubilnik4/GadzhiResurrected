using GadzhiMicrostation.Microstation.Implementations;
using GadzhiMicrostation.Models.Enum;
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
        public static string GetAttributeById(this Element element, ElementAttributes elementAttributes)
        {
            string controlName = AttributesElementsMicrostation.GetAttributeById(element, ElementAttributes.ControlName);
            return GetNameInCorrectCase(controlName);
        }

        /// <summary>
        /// Получить имя элемента из аттрибутов
        /// </summary>
        public static string GetAttributeControlName(this Element element)
        {
            return AttributesElementsMicrostation.GetAttributeById(element, ElementAttributes.ControlName);
        }

        /// <summary>
        /// Получить имя поля в корректном написании
        /// </summary>
        public static string GetNameInCorrectCase(string field)
        {
            return field?.Trim()?.ToUpper();
        }
    }
}
