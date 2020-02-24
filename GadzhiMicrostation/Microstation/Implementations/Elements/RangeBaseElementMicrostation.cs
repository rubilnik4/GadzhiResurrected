using GadzhiMicrostation.Extensions.Microstation;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Models.Coordinates;
using MicroStationDGN;
using System;

namespace GadzhiMicrostation.Microstation.Implementations.Elements
{
    /// <summary>
    /// Базовый класс для элементов находящихся в рамке
    /// </summary>
    public abstract class RangeBaseElementMicrostation : ElementMicrostation, IRangeBaseElementMicrostation
    {
        /// <summary>
        /// Экземпляр элемента Microstation
        /// </summary>
        private readonly Element _element;

        /// <summary>
        /// Необходимо ли сжатие в рамке
        /// </summary>
        public bool IsNeedCompress { get; set; }

        /// <summary>
        /// Вертикальное расположение
        /// </summary>
        public bool IsVertical { get; set; }

        public RangeBaseElementMicrostation(Element element,
                                           IOwnerContainerMicrostation ownerContainerMicrostation,
                                           bool isNeedCompress,
                                           bool isVertical)
            : base(element, ownerContainerMicrostation)
        {
            _element = element;
            IsNeedCompress = isNeedCompress;
            IsVertical = isVertical;
        }

        /// <summary>
        /// Координаты точки вставки
        /// </summary>
        public abstract PointMicrostation Origin { get; }

        /// <summary>
        /// Нижняя левая точка
        /// </summary>
        public PointMicrostation LowLeftPoint => _element.Range.Low.ToPointMicrostation();

        /// <summary>
        /// Размеры ячейки элемента в текущих координатах
        /// </summary>
        public RangeMicrostation Range => new RangeMicrostation(_element.Range.Low.ToPointMicrostation(),
                                                                 _element.Range.High.ToPointMicrostation(), IsVertical);

        /// <summary>
        /// Размеры ячейки элемента в стандартно заданных координатах
        /// </summary>
        private RangeMicrostation RangeAttribute => _element.GetAttributeRange(IsVertical);

        /// <summary>
        /// Ширина ячейки элемента в стандартно заданных координатах
        /// </summary>
        private double WidthAttribute => RangeAttribute.Width;

        /// <summary>
        /// Высота ячейки элемента в стандартно заданных координатах
        /// </summary>
        private double HeightAttribute => RangeAttribute.Height;

        /// <summary>
        /// Расположение элемента с учетом поворота в стандартно заданных координатах
        /// </summary>
        public PointMicrostation OriginPointWithRotationAttribute => RangeAttribute.OriginPointWithRotation;

        /// <summary>
        /// Ширина ячейки элемента в текущих координатах
        /// </summary>
        public double WidthAttributeInUnits => WidthAttribute * UnitScale;

        /// <summary>
        /// Высота ячейки элемента в текущих координатах
        /// </summary>
        public double HeightAttributeInUnits => HeightAttribute * UnitScale;

        /// <summary>
        /// Расположение элемента с учетом поворота в текущих координатах
        /// </summary>
        public PointMicrostation OriginPointWithRotationAttributeInUnits => OriginPointWithRotationAttribute.Multiply(UnitScale);

        /// <summary>
        /// Ширина элемента
        /// </summary>
        public double Width => !IsVertical ?
                                  Math.Abs(_element.Range.High.X - _element.Range.Low.X) :
                                  Math.Abs(_element.Range.High.Y - _element.Range.Low.Y);

        /// <summary>
        /// Высота элемента
        /// </summary>
        public double Height => !IsVertical ?
                                   Math.Abs(_element.Range.High.Y - _element.Range.Low.Y) :
                                   Math.Abs(_element.Range.High.X - _element.Range.Low.X);

        /// <summary>
        /// Вписать элемент в рамку
        /// </summary>
        public abstract bool CompressRange();
    }
}
