using GadzhiMicrostation.Extensions.Microstation;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Models.Implementations.Coordinates;
using GadzhiMicrostation.Models.Implementations.StampCollections;
using MicroStationDGN;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

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
            _textNodeElement = textNodeElement ?? throw new ArgumentNullException(nameof(textNodeElement));
        }

        /// <summary>
        /// Текстовые строки элемента
        /// </summary>
        private IList<string> _text;

        /// <summary>
        /// Текстовые строки элемента
        /// </summary>
        public IList<string> Text => _text ??= Enumerable.Range(1, _textNodeElement.TextLinesCount).
                                                          Select(lineIndex => _textNodeElement.TextLine[lineIndex]).
                                                          ToList();

        /// <summary>
        /// Однострочный текст
        /// </summary>
        public string TextInline => String.Join(" ", Text.ToArray());

        /// <summary>
        /// Координаты текстового поля
        /// </summary>
        public override PointMicrostation Origin => _textNodeElement.Origin.ToPointMicrostation();

        /// <summary>
        /// Переместить элемент
        /// </summary>
        public ITextNodeElementMicrostation Move(PointMicrostation offset) => Move<ITextNodeElementMicrostation>(offset);

        /// <summary>
        /// Повернуть элемент
        /// </summary>
        public ITextNodeElementMicrostation Rotate(PointMicrostation origin, double degree) =>
            Rotate<ITextNodeElementMicrostation>(origin, degree);

        /// <summary>
        /// Масштабировать элемент
        /// </summary>
        public ITextNodeElementMicrostation ScaleAll(PointMicrostation origin, PointMicrostation scaleFactor) =>
            ScaleAll<ITextNodeElementMicrostation>(origin, scaleFactor);

        /// <summary>
        /// Копировать элемент
        /// </summary>
        public override ITextNodeElementMicrostation Clone(bool isVertical) =>
            new TextNodeElementMicrostation(_textNodeElement, OwnerContainerMicrostation, IsNeedCompress, isVertical);

        /// <summary>
        /// Переместить текстовое поле
        /// </summary>
        protected override TElement Move<TElement>(PointMicrostation offset) => throw new NotImplementedException();

        /// <summary>
        /// Повернуть элемент
        /// </summary>
        protected override TElement Rotate<TElement>(PointMicrostation origin, double degree) => throw new NotImplementedException();

        /// <summary>
        /// Масштабировать текстовое поле
        /// </summary>
        protected override TElement ScaleAll<TElement>(PointMicrostation origin, PointMicrostation scaleFactor)
        {
            ChangeTextElementsInNode(textElement =>
            {
                textElement.ScaleAll(origin.ToPoint3d(), scaleFactor.X, scaleFactor.Y, scaleFactor.Z);
            });

            // необходимо сжатие самого элемента контейнера
            _textNodeElement.ScaleAll(Origin.ToPoint3d(), scaleFactor.X, scaleFactor.Y, scaleFactor.Z);
            _textNodeElement.Rewrite();
            return (TElement)Clone<ITextNodeElementMicrostation>();
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
    }
}
