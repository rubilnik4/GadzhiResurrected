using GadzhiMicrostation.Microstation.Implementations.Elements;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using MicroStationDGN;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Microstation.Converters
{
    /// <summary>
    /// Преобразование элемент библиотеки Microstation во внутреннюю обертку
    /// </summary>
    public static class ConvertMicrostationElements
    {
        /// <summary>
        /// Преобразовать элемент библиотеки Microstation во внутреннюю обертку
        /// </summary>      
        public static IElementMicrostation ToMicrostationElement(Element element, IOwnerMicrostation ownerContainerMicrostation) =>
            element switch
            {
                null => throw new ArgumentNullException(nameof(element)),
                _ when element.IsTextElement => new TextElementMicrostation(element.AsTextElement, ownerContainerMicrostation),
                _ when element.IsTextNodeElement => new TextNodeElementMicrostation(element.AsTextNodeElement, ownerContainerMicrostation),
                _ when element.IsCellElement => new CellElementMicrostation(element.AsCellElement, ownerContainerMicrostation),
                _ => throw new InvalidEnumArgumentException(nameof(Element))
            };

        /// <summary>
        /// Возможно ли конвертировать элемент Microstation в элемент-обертку
        /// </summary>    
        public static bool IsConvertibleToMicrostation(Element element) =>
            element?.IsTextElement == true ||
            element?.IsTextNodeElement == true ||
            element?.IsCellElement == true;
    }
}
