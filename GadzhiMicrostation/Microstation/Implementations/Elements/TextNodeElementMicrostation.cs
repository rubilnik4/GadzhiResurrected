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
        public bool CompressRange()
        {
            bool isComressed = false;

            if (IsNeedCompress == true)
            {
                double scaleX = 1;
                double scaleY = 1;

                if (WidthAttributeInUnits * StampAdditionalParameters.CompressionRatioTextNode < Width)
                {
                    scaleX = (WidthAttributeInUnits / Width) * StampAdditionalParameters.CompressionRatioTextNode;
                }

                if (HeightAttributeInUnits * StampAdditionalParameters.CompressionRatioTextNode < Height &&
                    _textNodeElement.TextLinesCount > 1) // коррекция высоты при многострочном элементе
                {
                    scaleY = (HeightAttributeInUnits / Height) * StampAdditionalParameters.CompressionRatioTextNode;
                }

                if (scaleX != 1 || scaleY != 1)
                {
                    ChangeTextElementsInNode(textElement =>
                    {
                        textElement.ScaleAll(_textNodeElement.Origin, scaleX, scaleY, 1);
                    });

                    isComressed = true;
                }
            }

            return isComressed;
        }

        /// <summary>
        /// Присвоить параметры всем текстовым элементам
        /// </summary>       
        private void ChangeTextElementsInNode(Action<TextElement> changeTextElement)
        {
            if (changeTextElement != null)
            {
                ElementEnumerator elementEnumerator = _textNodeElement.GetSubElements();
                while (elementEnumerator.MoveNext())
                {
                    TextElement textElement = (TextElement)elementEnumerator.Current;
                    changeTextElement.Invoke(textElement);
                    textElement.Rewrite();
                }
            }

        }
    }
}
