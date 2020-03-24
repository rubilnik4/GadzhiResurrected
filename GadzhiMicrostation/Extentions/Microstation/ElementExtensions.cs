using GadzhiMicrostation.Microstation.Converters;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using MicroStationDGN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Extentions.Microstation
{
    /// <summary>
    /// Методы расширения для элементов Microstation
    /// </summary>
    public static class ElementExtensions
    {
        /// <summary>
        /// Преобразовать элемент Microstation в элемент-обертку
        /// </summary>      
        public static IElementMicrostation ToElementMicrostation(this Element element, IOwnerMicrostation owner) =>
            ConvertMicrostationElements.ConvertToMicrostationElement(element, owner);

        /// <summary>
        /// Возможно ли конвертировать элемент Microstation в элемент-обертку
        /// </summary>    
        public static bool IsConvertableToMicrostation(this Element element) =>
            ConvertMicrostationElements.IsConvertableToMicrostation(element);

        /// <summary>
        /// Получить подэлементы ячейки
        /// </summary>       
        public static IEnumerable<Element> GetCellSubElements(this CellElement cellElement)
        {
            ElementEnumerator elementEnumerator = cellElement?.GetSubElements();
            while (elementEnumerator?.MoveNext() == true)
            {
                yield return (Element)elementEnumerator.Current;
            }
        }
    }
}
