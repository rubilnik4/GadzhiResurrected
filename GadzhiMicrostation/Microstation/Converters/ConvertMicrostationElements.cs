using GadzhiMicrostation.Microstation.Implementations.Elements;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using MicroStationDGN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Microstation.Converters
{
    /// <summary>
    /// Преобразование элемент бибилиотеки Microstation во внутреннюю обертку
    /// </summary>
    public static class ConvertMicrostationElements
    {
        /// <summary>
        /// Преобразовать элемент бибилиотеки Microstation во внутреннюю обертку
        /// </summary>      
        public static IElementMicrostation ConvertToMicrostationElement(Element element, IOwnerMicrostation ownerContainerMicrostation)
        {
            IElementMicrostation elementMicrostation = null;

            if (IsConvertableToMicrostation(element))
            {
                if (element?.IsTextElement == true)
                {
                    elementMicrostation = new TextElementMicrostation((TextElement)element, ownerContainerMicrostation);
                }
                else if (element?.IsTextNodeElement == true)
                {
                    elementMicrostation = new TextNodeElementMicrostation((TextNodeElement)element, ownerContainerMicrostation);
                }
                else if (element?.IsCellElement == true)
                {
                    elementMicrostation = new CellElementMicrostation((CellElement)element, ownerContainerMicrostation);
                }
            }

            return elementMicrostation;
        }

        /// <summary>
        /// Возможно ли конвертировать элемент Microstation в элемент-обертку
        /// </summary>    
        public static bool IsConvertableToMicrostation(Element element) =>
            element?.IsTextElement == true ||
            element?.IsTextNodeElement == true ||
            element?.IsCellElement == true;
    }
}
