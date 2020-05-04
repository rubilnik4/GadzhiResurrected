using System.Collections.Generic;
using GadzhiMicrostation.Microstation.Converters;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using MicroStationDGN;

namespace GadzhiMicrostation.Extensions.Microstation
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
            ConvertMicrostationElements.ToMicrostationElement(element, owner);

        /// <summary>
        /// Возможно ли конвертировать элемент Microstation в элемент-обертку
        /// </summary>    
        public static bool IsConvertibleToMicrostation(this Element element) =>
            ConvertMicrostationElements.IsConvertibleToMicrostation(element);

        /// <summary>
        /// Получить дочерние элементы ячейки
        /// </summary>       
        public static IEnumerable<Element> GetCellSubElements(this CellElement cellElement)
        {
            var elementEnumerator = cellElement?.GetSubElements();
            while (elementEnumerator?.MoveNext() == true)
            {
                yield return (Element)elementEnumerator.Current;
            }
        }

        /// <summary>
        /// Получить список моделей
        /// </summary>
        public static IEnumerable<ModelReference> ToIEnumerable(this ModelReferences modelReferences)
        {
            if (modelReferences == null) yield break;

            foreach (ModelReference model in modelReferences)
            {
                yield return model;
            }
        }
    }
}
