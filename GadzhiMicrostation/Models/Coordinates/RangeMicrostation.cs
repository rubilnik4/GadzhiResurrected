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
        /// Левая верхняя точка
        /// </summary>
        private readonly PointMicrostaion highLeftPoint;

        /// <summary>
        /// Правая нижняя точка
        /// </summary>
        private readonly PointMicrostaion lowRightPoint;

        /// <summary>
        /// Вертикальное расположение
        /// </summary>
        public bool IsVertical { get; }

        public RangeMicrostation()
        {
            highLeftPoint = new PointMicrostaion(0, 0, 0);
            lowRightPoint = new PointMicrostaion(0, 0, 0);
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
                    highLeftPoint = new PointMicrostaion(points[0], points[1], points[2]);
                    lowRightPoint = new PointMicrostaion(points[3], points[4], points[5]);
                }
            }
        }

        /// <summary>
        /// Ширина
        /// </summary>
        public double Width => !IsVertical ?
                               Math.Abs(highLeftPoint?.X - lowRightPoint.X ?? 0):
                               Math.Abs(highLeftPoint?.Y - lowRightPoint.Y ?? 0);

        /// <summary>
        /// Высота
        /// </summary>
        public double Height => !IsVertical ? 
                                Math.Abs(highLeftPoint?.Y - lowRightPoint.Y ?? 0):
                                Math.Abs(highLeftPoint?.X- lowRightPoint.X ?? 0);

        /// <summary>
        /// Корректны ли значения координат
        /// </summary>
        public bool IsValid => highLeftPoint != null && lowRightPoint != null &&
                               Width > 0 && Height > 0;
    }
}
