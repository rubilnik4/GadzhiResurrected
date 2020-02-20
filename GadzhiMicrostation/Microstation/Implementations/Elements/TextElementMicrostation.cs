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
                                       IOwnerContainerMicrostation ownerContainerMicrostation)
            : this(textElement, ownerContainerMicrostation, true, false)
        {
        }

        public TextElementMicrostation(TextElement textElement,
                                       IOwnerContainerMicrostation ownerContainerMicrostation,
                                       bool isNeedCompress,
                                       bool isVertical)
            : base((Element)textElement, ownerContainerMicrostation, isNeedCompress, isVertical)
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
        public bool CompressRange()
        {
            bool isComressed = false;

            if (IsNeedCompress == true)
            {
                if (WidthAttributeInUnits * StampAdditionalParameters.CompressionRatioText < Width)
                {
                    double compressionLevel = (WidthAttributeInUnits / Width) * StampAdditionalParameters.CompressionRatioText;
                    _textElement.TextStyle.Width *= compressionLevel;
                    _textElement.Rewrite();

                    isComressed = true;
                }
            }

            return isComressed;
        }
    }
}
