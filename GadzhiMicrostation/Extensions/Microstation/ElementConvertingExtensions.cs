using GadzhiMicrostation.Microstation.Interfaces.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Extensions.Microstation
{
    /// <summary>
    /// Преобразование базового элемента Microstation в другие
    /// </summary>
    public static class ElementConvertingExtensions
    {
        /// <summary>
        /// Преобразование базового элемента Microstation в текстовые элемент
        /// </summary>       
        public static ITextElementMicrostation AsTextElementMicrostation(this IElementMicrostation elementMicrostation)
        {
            if (elementMicrostation.GetType() == typeof(ITextElementMicrostation))
            {
                return (ITextElementMicrostation)elementMicrostation;
            }
            return null;
        }

        /// <summary>
        /// Является ли базовый элемент Microstation текстовым
        /// </summary>       
        public static bool IsTextElementMicrostation(this IElementMicrostation elementMicrostation)
        {
            return elementMicrostation.GetType() == typeof(ITextElementMicrostation);            
        }
    }
}
