using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Models.Coordinates
{
    /// <summary>
    /// Координаты и размеры
    /// </summary>
    public class RangeMicrostation
    {
        /// <summary>
        /// Координаты верхнего левого угла
        /// </summary>
        private readonly PointMicrostation _highLeftPoint;

        /// <summary>
        /// Координаты правого нижнего угла
        /// </summary>
        private readonly PointMicrostation _lowRightPoint;

        /// <summary>
        /// Вертикальное расположение
        /// </summary>
        public bool IsVertical { get; }

        public RangeMicrostation()
        {
            _highLeftPoint = new PointMicrostation(0, 0, 0);
            _lowRightPoint = new PointMicrostation(0, 0, 0);
            IsVertical = false;
        }

        public RangeMicrostation(PointMicrostation highLeftPoint,
                                 PointMicrostation lowRightPoint,
                                 bool isVertical)        
        {
            _highLeftPoint = highLeftPoint;
            _lowRightPoint = lowRightPoint;
            IsVertical = isVertical;
        }

        public RangeMicrostation(IList<string> pointsAttribute, bool isVertical)
            : this()
        {
            IsVertical = isVertical;
            if (pointsAttribute?.Count == 6) //включая z координаты
            {
                IList<double> points = pointsAttribute?.Select(p => Double.Parse(p)).ToList();
                if (points?.Count == pointsAttribute.Count)
                {
                    _highLeftPoint = new PointMicrostation(points[0], points[1], points[2]);
                    _lowRightPoint = new PointMicrostation(points[3], points[4], points[5]);
                }
            }
        }

        /// <summary>
        /// Ширина
        /// </summary>
        public double Width => !IsVertical ?
                               Math.Abs(_highLeftPoint?.X - _lowRightPoint.X ?? 0):
                               Math.Abs(_highLeftPoint?.Y - _lowRightPoint.Y ?? 0);

        /// <summary>
        /// Высота
        /// </summary>
        public double Height => !IsVertical ? 
                                Math.Abs(_highLeftPoint?.Y - _lowRightPoint.Y ?? 0):
                                Math.Abs(_highLeftPoint?.X- _lowRightPoint.X ?? 0);
      

        /// <summary>
        /// Корректны ли значения координат
        /// </summary>
        public bool IsValid => _highLeftPoint != null && _lowRightPoint != null &&
                               Width > 0 && Height > 0;

        /// <summary>
        /// точка вставки элемента с учетом поворота
        /// </summary>
        public PointMicrostation OriginPointWithRotation => !IsVertical ?
                                                            _highLeftPoint :
                                                            new PointMicrostation(_highLeftPoint.X, _lowRightPoint.Y, 0);
    }
}
