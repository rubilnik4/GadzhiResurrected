using GadzhiMicrostation.Extensions.Microstation;
using GadzhiMicrostation.Microstation.Interfaces;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Models.Coordinates;
using GadzhiMicrostation.Models.Enum;
using GadzhiMicrostation.Models.StampCollections;
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
    public class TextElementMicrostation : TextBaseElementMicrostation, ITextElementMicrostation
    {
        /// <summary>
        /// Экземпляр текстового элемента Microstation
        /// </summary>
        private readonly TextElement _textElement;

        public TextElementMicrostation(TextElement textElement,
                                       IElementMicrostation ownerElementMicrostation)
            : this(textElement, ownerElementMicrostation, true, false)
        {
        }

        public TextElementMicrostation(TextElement textElement,
                                       IElementMicrostation ownerElementMicrostation,
                                       bool isNeedCompress,
                                       bool isVertical)
            : base((Element)textElement, ownerElementMicrostation, isNeedCompress, isVertical)
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
            if (IsNeedCompress == true)
            {
                if (WidthAttributeInUnits * StampAdditionalParameters.CompressionRatio < Width)
                {
                    double compressionLevel = (WidthAttributeInUnits / Width) * StampAdditionalParameters.CompressionRatio;
                    _textElement.TextStyle.Width *= compressionLevel;
                    _textElement.Rewrite();
                }
            }
        }
    }
}
