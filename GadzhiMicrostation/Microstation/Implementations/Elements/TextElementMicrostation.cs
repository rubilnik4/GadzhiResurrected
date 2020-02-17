using GadzhiMicrostation.Microstation.Interfaces.Elements;
using MicroStationDGN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Microstation.Implementations.Elements
{
    /// <summary>
    /// Текстовый элемент типа Microstation
    /// </summary>
    public class TextElementMicrostation : ElementMicrostation, ITextElementMicrostation
    {
        /// <summary>
        /// Экземпляр текстового элемента Microstation
        /// </summary>
        private readonly TextElement _textElement;

        public TextElementMicrostation(TextElement textElement)
            :base ((Element)textElement)
        {
            _textElement = textElement;
        }

        /// <summary>
        /// Текст элемента
        /// </summary>
        public string Text => _textElement?.Text;

        /// <summary>
        /// Вписать текстовый элемент в рамку
        /// </summary>
        public void CompressRange()
        {
           
        }
    }
}
