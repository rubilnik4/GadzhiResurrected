using GadzhiMicrostation.Microstation.Interfaces;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Models.StampCollections;
using MicroStationDGN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Microstation.Implementations.Elements
{
    public class TextNodeElementMicrostation : TextBaseElementMicrostation, ITextNodeElementMicrostation
    {
        /// <summary>
        /// Экземпляр текстового элемента Microstation
        /// </summary>
        private readonly TextNodeElement _textNodeElement;

        public TextNodeElementMicrostation(TextNodeElement textNodeElement,
                                           IElementMicrostation ownerElementMicrostation)
          : this(textNodeElement, ownerElementMicrostation, true, false)
        {
        }

        public TextNodeElementMicrostation(TextNodeElement textNodeElement,
                                           IElementMicrostation ownerElementMicrostation,
                                           bool isNeedCompress,
                                           bool isVertical)
            : base((Element)textNodeElement, ownerElementMicrostation, isNeedCompress, isVertical)
        {
            _textNodeElement = textNodeElement;
        }        

        /// <summary>
        /// Вписать текстовое поле в рамку
        /// </summary>
        public void CompressRange()
        {
            if (IsNeedCompress == true)
            {
                double scaleX = 1;
                double scaleY = 1;

                if (_textNodeElement.TextLine[1].ToLower().Contains( "обустройство"))
                {

                }

                if (WidthAttributeInUnits * StampAdditionalParameters.CompressionRatio < Width)
                {
                    scaleX = (WidthAttributeInUnits / Width) * StampAdditionalParameters.CompressionRatio;
                }
                if (HeightAttributeInUnits * StampAdditionalParameters.CompressionRatio < Height)
                {
                    scaleY = (HeightAttributeInUnits / Height) * StampAdditionalParameters.CompressionRatio;
                }

                if (scaleX != 1 || scaleY != 1)
                {
                    _textNodeElement.ScaleAll(_textNodeElement.Origin, scaleX, scaleY, 1);                    
                    _textNodeElement.Rewrite();                    
                }
            }
        }
    }
}
