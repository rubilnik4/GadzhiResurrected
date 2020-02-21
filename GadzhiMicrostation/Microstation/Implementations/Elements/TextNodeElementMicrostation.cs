using GadzhiMicrostation.Extensions.Microstation;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Models.Coordinates;
using GadzhiMicrostation.Models.StampCollections;
using MicroStationDGN;
using System;

namespace GadzhiMicrostation.Microstation.Implementations.Elements
{
    public class TextNodeElementMicrostation : RangeBaseElementMicrostation, ITextNodeElementMicrostation
    {
        /// <summary>
        /// Экземпляр текстового элемента Microstation
        /// </summary>
        private readonly TextNodeElement _textNodeElement;

        public TextNodeElementMicrostation(TextNodeElement textNodeElement,
                                           IOwnerContainerMicrostation ownerContainerMicrostation)
          : this(textNodeElement, ownerContainerMicrostation, true, false)
        {
        }

        public TextNodeElementMicrostation(TextNodeElement textNodeElement,
                                           IOwnerContainerMicrostation ownerContainerMicrostation,
                                           bool isNeedCompress,
                                           bool isVertical)
            : base((Element)textNodeElement, ownerContainerMicrostation, isNeedCompress, isVertical)
        {
            _textNodeElement = textNodeElement;
        }

        /// <summary>
        /// Координаты текстового поля
        /// </summary>
        public override PointMicrostation Origin => _textNodeElement.Origin.ToPointMicrostation();

        /// <summary>
        /// Переместить текстовое поле
        /// </summary>
        public override void Move(PointMicrostation offset)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Масштабировать текстовое поле
        /// </summary>
        public override void ScaleAll(PointMicrostation origin, PointMicrostation scaleFactor)
        {
            ChangeTextElementsInNode(textElement =>
            {
                textElement.ScaleAll(origin.ToPoint3d(), scaleFactor.X, scaleFactor.Y, scaleFactor.Z);
            });
        }

        /// <summary>
        /// Вписать текстовое поле в рамку
        /// </summary>
        public override bool CompressRange()
        {
            bool isComressed = false;

            if (IsNeedCompress == true)
            {
                var scaleFactor = new PointMicrostation(1, 1, 1);

                if (WidthAttributeInUnits * StampAdditionalParameters.CompressionRatioTextNode < Width)
                {
                    scaleFactor.X = (WidthAttributeInUnits / Width) * StampAdditionalParameters.CompressionRatioTextNode;
                }

                if (HeightAttributeInUnits * StampAdditionalParameters.CompressionRatioTextNode < Height &&
                    _textNodeElement.TextLinesCount > 1) // коррекция высоты при многострочном элементе
                {
                    scaleFactor.Y = (HeightAttributeInUnits / Height) * StampAdditionalParameters.CompressionRatioTextNode;
                }

                if (scaleFactor.X != 1 || scaleFactor.Y != 1)
                {
                    ScaleAll(Origin, scaleFactor);

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
