using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using GadzhiMicrostation.Extensions.Microstation;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Models.Implementations.Coordinates;
using MicroStationDGN;

namespace GadzhiMicrostation.Microstation.Implementations.Elements
{
    /// <summary>
    /// Элемент линии типа Microstation
    /// </summary>
    public class LineElementMicrostation : ElementMicrostation, ILineElementMicrostation
    {
        /// <summary>
        /// Экземпляр линии Microstation определяющей штамп
        /// </summary>
        private readonly LineElement _lineElement;

        [SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
        public LineElementMicrostation(LineElement lineElement, IOwnerMicrostation ownerContainerMicrostation)
           : base((Element)lineElement, ownerContainerMicrostation)
        {
            _lineElement = lineElement;
        }

        /// <summary>
        /// Начальная точка
        /// </summary>
        private PointMicrostation? _startPoint;

        /// <summary>
        /// Начальная точка
        /// </summary>
        public PointMicrostation StartPoint => _startPoint ??= _lineElement.StartPoint.ToPointMicrostation();

        /// <summary>
        /// Начальная точка
        /// </summary>
        private PointMicrostation? _endPoint;

        /// <summary>
        /// Конечная точка
        /// </summary>
        public PointMicrostation EndPoint => _endPoint ??= _lineElement.EndPoint.ToPointMicrostation();

        /// <summary>
        /// Переместить элемент
        /// </summary>
        public ILineElementMicrostation Move(PointMicrostation offset) => Move<ILineElementMicrostation>(offset);

        /// <summary>
        /// Повернуть элемент
        /// </summary>
        public ILineElementMicrostation Rotate(PointMicrostation origin, double degree) => 
            Rotate<ILineElementMicrostation>(origin, degree);

        /// <summary>
        /// Масштабировать элемент
        /// </summary>
        public ILineElementMicrostation ScaleAll(PointMicrostation origin, PointMicrostation scaleFactor) =>
            ScaleAll<ILineElementMicrostation>(origin, scaleFactor);

        /// <summary>
        /// Копировать элемент
        /// </summary>
        protected override TElement Clone<TElement>()=>
            (TElement)(ILineElementMicrostation) new LineElementMicrostation(_lineElement, OwnerContainerMicrostation);
    }
}
