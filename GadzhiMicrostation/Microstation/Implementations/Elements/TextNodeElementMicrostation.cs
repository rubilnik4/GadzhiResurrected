using GadzhiMicrostation.Extensions.Microstation;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Models.Implementations.Coordinates;
using GadzhiMicrostation.Models.Implementations.StampCollections;
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
        /// Повернуть элемент
        /// </summary>
        public override void Rotate(PointMicrostation origin, double degree)
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
            /// необходимо сжатие самого элемента контейнера
            _textNodeElement.ScaleAll(Origin.ToPoint3d(), scaleFactor.X, scaleFactor.Y, scaleFactor.Z);
            _textNodeElement.Rewrite();
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

                if (WidthAttributeWithRotationInUnits * StampAdditionalParameters.CompressionRatioTextNode < WidthWithRotation)
                {
                    scaleFactor.X = (WidthAttributeWithRotationInUnits / WidthWithRotation) * StampAdditionalParameters.CompressionRatioTextNode;
                }

                if (HeightAttributeWithRotationInUnits * StampAdditionalParameters.CompressionRatioTextNode < HeightWithRotation &&
                    _textNodeElement.TextLinesCount > 1) // коррекция высоты только при многострочном элементе
                {
                    scaleFactor.Y = (HeightAttributeWithRotationInUnits / HeightWithRotation) * StampAdditionalParameters.CompressionRatioTextNode;
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
