using GadzhiMicrostation.Extensions.Microstation;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Models.Implementations.Coordinates;
using GadzhiMicrostation.Models.Implementations.StampCollections;
using MicroStationDGN;
using System;
using System.Diagnostics.CodeAnalysis;

namespace GadzhiMicrostation.Microstation.Implementations.Elements
{
    public class TextNodeElementMicrostation : RangeBaseElementMicrostation<ITextNodeElementMicrostation>, ITextNodeElementMicrostation
    {
        /// <summary>
        /// Экземпляр текстового элемента Microstation
        /// </summary>
        private readonly TextNodeElement _textNodeElement;

        public TextNodeElementMicrostation(TextNodeElement textNodeElement, IOwnerMicrostation ownerContainerMicrostation)
          : this(textNodeElement, ownerContainerMicrostation, true, false)
        { }

        [SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
        public TextNodeElementMicrostation(TextNodeElement textNodeElement, IOwnerMicrostation ownerContainerMicrostation,
                                           bool isNeedCompress, bool isVertical)
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
        public override IElementMicrostation Move(PointMicrostation offset) => throw new NotImplementedException();

        /// <summary>
        /// Повернуть элемент
        /// </summary>
        public override IElementMicrostation Rotate(PointMicrostation origin, double degree) => throw new NotImplementedException();

        /// <summary>
        /// Масштабировать текстовое поле
        /// </summary>
        public override IElementMicrostation ScaleAll(PointMicrostation origin, PointMicrostation scaleFactor)
        {
            ChangeTextElementsInNode(textElement =>
            {
                textElement.ScaleAll(origin.ToPoint3d(), scaleFactor.X, scaleFactor.Y, scaleFactor.Z);
            });
            _textNodeElement.ScaleAll(Origin.ToPoint3d(), scaleFactor.X, scaleFactor.Y, scaleFactor.Z);  // необходимо сжатие самого элемента контейнера
            _textNodeElement.Rewrite();
            return this;
        }

        /// <summary>
        /// Вписать текстовое поле в рамку
        /// </summary>
        public override bool CompressRange()
        {
            if (!IsNeedCompress || !IsValidToCompress) return false;

            var scaleFactor = new PointMicrostation(1, 1, 1);

            if (WidthAttributeWithRotationInUnits * StampSettingsMicrostation.CompressionRatioTextNode < WidthWithRotation)
            {
                double scaleFactorX = (WidthAttributeWithRotationInUnits / WidthWithRotation) * StampSettingsMicrostation.CompressionRatioTextNode;
                scaleFactor = new PointMicrostation(scaleFactorX, scaleFactor.Y, scaleFactor.Z);
            }

            if (HeightAttributeWithRotationInUnits * StampSettingsMicrostation.CompressionRatioTextNode < HeightWithRotation &&
                _textNodeElement.TextLinesCount > 1) // коррекция высоты только при многострочном элементе
            {
                double scaleFactorY = (HeightAttributeWithRotationInUnits / HeightWithRotation) * StampSettingsMicrostation.CompressionRatioTextNode;
                scaleFactor = new PointMicrostation(scaleFactor.X, scaleFactorY, scaleFactor.Z);
            }

            if (PointMicrostation.CompareCoordinate(scaleFactor.X, 1) &&
                PointMicrostation.CompareCoordinate(scaleFactor.Y, 1)) return false;

            ScaleAll(Origin, scaleFactor);
            return true;

        }

        /// <summary>
        /// Присвоить параметры всем текстовым элементам
        /// </summary>       
        private void ChangeTextElementsInNode(Action<TextElement> changeTextElement)
        {
            var elementEnumerator = _textNodeElement.GetSubElements();
            while (elementEnumerator.MoveNext())
            {
                var textElement = (TextElement)elementEnumerator.Current;
                changeTextElement?.Invoke(textElement);
                textElement.Rewrite();
            }
        }

        /// <summary>
        /// Копировать элемент
        /// </summary>     
        public override ITextNodeElementMicrostation Copy(bool isVertical) =>
            new TextNodeElementMicrostation(_textNodeElement, OwnerContainerMicrostation, IsNeedCompress, isVertical);
    }
}
