using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

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
        private PointMicrostation _lowLeftPoint;

        /// <summary>
        /// Координаты правого нижнего угла
        /// </summary>
        private PointMicrostation _highRightPoint;

        /// <summary>
        /// Вертикальное расположение
        /// </summary>
        public bool IsVertical { get; }

        public RangeMicrostation(PointMicrostation lowLeftPoint,
                                 PointMicrostation highRightPoint,
                                 bool isVertical)
        {
            _lowLeftPoint = lowLeftPoint;
            _highRightPoint = highRightPoint;
            IsVertical = isVertical;
        }

        public RangeMicrostation(IList<string> pointsAttribute, bool isVertical)
        {
            IsVertical = isVertical;
            if (pointsAttribute?.Count == 6) //включая z координаты
            {
                IList<double> points = pointsAttribute?.Select(p => Double.Parse(p, CultureInfo.CurrentCulture)).ToList();
                if (points?.Count == pointsAttribute.Count)
                {
                    _lowLeftPoint = new PointMicrostation(Math.Min(points[0], points[3]), Math.Min(points[1], points[4]), points[2]);
                    _highRightPoint = new PointMicrostation(Math.Max(points[0], points[3]), Math.Max(points[1], points[4]), points[5]);
                }
            }
        }

        /// <summary>
        /// Ширина
        /// </summary>
        public double Width => !IsVertical ?
                               Math.Abs(_lowLeftPoint.X - _highRightPoint.X) :
                               Math.Abs(_lowLeftPoint.Y - _highRightPoint.Y);

        /// <summary>
        /// Высота
        /// </summary>
        public double Height => !IsVertical ?
                                Math.Abs(_lowLeftPoint.Y - _highRightPoint.Y) :
                                Math.Abs(_lowLeftPoint.X - _highRightPoint.X);


        /// <summary>
        /// Корректны ли значения координат
        /// </summary>
        public bool IsValid => _lowLeftPoint != null && _highRightPoint != null &&
                               Width > 0 && Height > 0;

        /// <summary>
        /// Точка вставки элемента с учетом поворота
        /// </summary>
        public PointMicrostation OriginPointWithRotation => !IsVertical ?
                                                            _lowLeftPoint :
                                                            new PointMicrostation(_highRightPoint.X, _lowLeftPoint.Y);

        /// <summary>
        /// Преобразовать границы в массив точек
        /// </summary>        
        public IList<PointMicrostation> ToPointsMicrostation() =>
            new List<PointMicrostation>()
            {
                _lowLeftPoint,
                new PointMicrostation(_lowLeftPoint.X , _highRightPoint.Y),
                _highRightPoint,
                 new PointMicrostation(_highRightPoint.X , _lowLeftPoint.Y)
            };

        /// <summary>
        /// Центральная точка
        /// </summary>
        public PointMicrostation OriginCenter =>
            new PointMicrostation(_lowLeftPoint.X + Math.Abs(_lowLeftPoint.X - _highRightPoint.X) / 2,
                                  _lowLeftPoint.Y - Math.Abs(_lowLeftPoint.Y - _highRightPoint.Y) / 2,
                                  0);

        /// <summary>
        /// Сдвиг по координатам
        /// </summary>       
        public RangeMicrostation Offset(PointMicrostation offset) =>
             new RangeMicrostation(_lowLeftPoint.Add(offset), _highRightPoint.Add(offset), IsVertical);


        /// <summary>
        /// Масштабирование
        /// </summary>       
        public RangeMicrostation Scale(double scaleFactor) =>
             new RangeMicrostation(_lowLeftPoint.Multiply(scaleFactor), _highRightPoint.Multiply(scaleFactor), IsVertical);
    }
}
