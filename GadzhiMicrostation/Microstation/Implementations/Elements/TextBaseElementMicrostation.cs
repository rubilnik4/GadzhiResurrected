using GadzhiMicrostation.Extensions.Microstation;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Models.Coordinates;
using MicroStationDGN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Microstation.Implementations.Elements
{
    /// <summary>
    /// Базовый класс для текстовыйх полей
    /// </summary>
    public abstract class TextBaseElementMicrostation : ElementMicrostation
    {
        /// <summary>
        /// Экземпляр элемента Microstation
        /// </summary>
        private readonly Element _element;

        /// <summary>
        /// Необходимо ли сжатие в рамке
        /// </summary>
        protected bool IsNeedCompress { get; }

        /// <summary>
        /// Вертикальное расположение
        /// </summary>
        protected bool IsVertical { get; }

        public TextBaseElementMicrostation(Element element,
                                           IElementMicrostation ownerElementMicrostation,
                                           bool isNeedCompress,
                                           bool isVertical)
            : base(element, ownerElementMicrostation)
        {
            _element = element;
            IsNeedCompress = isNeedCompress;
            IsVertical = isVertical;
        }

        /// <summary>
        /// Размеры ячейки элемента в стандартно заданных координатах
        /// </summary>
        protected RangeMicrostation RangeAttribute => _element.GetAttributeRange(IsVertical);

        /// <summary>
        /// Ширина ячейки элемента в стандартно заданных координатах
        /// </summary>
        protected double WidthAttribute => RangeAttribute.Width;

        /// <summary>
        /// Высота ячейки элемента в стандартно заданных координатах
        /// </summary>
        protected double HeightAttribute => RangeAttribute.Height;

        /// <summary>
        /// Ширина ячейки элемента в текущих координатах
        /// </summary>
        protected double WidthAttributeInUnits => WidthAttribute * UnitScale;

        /// <summary>
        /// Высота ячейки элемента в текущих координатах
        /// </summary>
        protected double HeightAttributeInUnits => HeightAttribute * UnitScale;

        /// <summary>
        /// Ширина элемента
        /// </summary>
        protected double Width => !IsVertical ?
                                Math.Abs(_element.Range.High.X - _element.Range.Low.X) :
                                Math.Abs(_element.Range.High.Y - _element.Range.Low.Y);

        /// <summary>
        /// Высота элемента
        /// </summary>
        protected double Height => !IsVertical ?
                                   Math.Abs(_element.Range.High.Y - _element.Range.Low.Y) :
                                   Math.Abs(_element.Range.High.X - _element.Range.Low.X);


    }
}
