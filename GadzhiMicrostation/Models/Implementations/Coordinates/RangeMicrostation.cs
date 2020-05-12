using GadzhiApplicationCommon.Extensions.Functional;
using GadzhiApplicationCommon.Extensions.Functional.Result;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiMicrostation.Models.Enums;
using GadzhiMicrostation.Models.Implementations.StampCollections;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using GadzhiApplicationCommon.Models.Enums;

namespace GadzhiMicrostation.Models.Implementations.Coordinates
{
    /// <summary>
    /// Координаты и размеры
    /// </summary>
    public readonly struct RangeMicrostation : IEquatable<RangeMicrostation>
    {
        public RangeMicrostation(PointMicrostation lowLeftPoint, PointMicrostation highRightPoint)
        {
            LowLeftPoint = lowLeftPoint;
            HighRightPoint = highRightPoint;
        }

        public RangeMicrostation(IList<double> points)
        {
            if (points == null) throw new ArgumentNullException(nameof(points));
            if (points.Count != 6) throw new ArgumentOutOfRangeException(nameof(points));

            LowLeftPoint = new PointMicrostation(Math.Min(points[0], points[3]),
                                                 Math.Min(points[1], points[4]), points[2]);

            HighRightPoint = new PointMicrostation(Math.Max(points[0], points[3]),
                                                   Math.Max(points[1], points[4]), points[5]);
        }

        /// <summary>
        /// Координаты верхнего левого угла
        /// </summary>
        public PointMicrostation LowLeftPoint { get; }

        /// <summary>
        /// Координаты правого нижнего угла
        /// </summary>
        public PointMicrostation HighRightPoint { get; }

        /// <summary>
        /// Ширина с учетом поворота
        /// </summary>
        public double Width => Math.Abs(LowLeftPoint.X - HighRightPoint.X);

        /// <summary>
        /// Высота с учетом поворота
        /// </summary>
        public double Height => Math.Abs(LowLeftPoint.Y - HighRightPoint.Y);

        /// <summary>
        /// Корректны ли значения координат
        /// </summary>
        public bool IsValid => Width > 0 && Height > 0;

        /// <summary>
        /// Точка вставки элемента с учетом поворота
        /// </summary>
        public PointMicrostation OriginPoint => LowLeftPoint;

        /// <summary>
        /// Преобразовать границы в массив точек
        /// </summary>        
        public IList<PointMicrostation> ToPointsMicrostation() =>
            new List<PointMicrostation>()
            {
                LowLeftPoint,
                new PointMicrostation(LowLeftPoint.X , HighRightPoint.Y),
                HighRightPoint,
                 new PointMicrostation(HighRightPoint.X , LowLeftPoint.Y)
            };

        /// <summary>
        /// Центральная точка
        /// </summary>
        public PointMicrostation OriginCenter =>
            new PointMicrostation(LowLeftPoint.X + Math.Abs(LowLeftPoint.X - HighRightPoint.X) / 2,
                                  LowLeftPoint.Y + Math.Abs(LowLeftPoint.Y - HighRightPoint.Y) / 2,
                                  0);

        /// <summary>
        /// Сдвиг по координатам
        /// </summary>       
        public RangeMicrostation Offset(PointMicrostation offset) =>
             new RangeMicrostation(LowLeftPoint.Add(offset), HighRightPoint.Add(offset));

        /// <summary>
        /// Масштабирование
        /// </summary>       
        public RangeMicrostation Scale(double scaleFactor) =>
             new RangeMicrostation(LowLeftPoint.Multiply(scaleFactor), HighRightPoint.Multiply(scaleFactor));

        /// <summary>
        /// Преобразовать строку в диапазон
        /// </summary>    
        public static IResultAppValue<RangeMicrostation> StringToRange(string rangeInString) =>
            StampSettingsMicrostation.SeparateAttributeValue(rangeInString).
            WhereContinue(ValidateRangeString,
                okFunc: rangeListString => new ResultAppValue<IList<string>>(rangeListString),
                badFunc: _ => new ErrorApplication(ErrorApplicationType.RangeNotValid, "Некорректный диапазон координат из атрибутов").
                              ToResultApplicationValue<IList<string>>()).
            ResultValueOk(rangeListString => rangeListString.
                                             Select(pointsString => Double.Parse(pointsString, CultureInfo.CurrentCulture))).
            ResultValueOk(points => new RangeMicrostation(points.ToList()));
     
        /// <summary>
        /// Проверить строку на возможность преобразования в диапазон
        /// </summary>   
        public static bool ValidateRangeString(IList<string> rangeListString) =>
            rangeListString?.Count == 6 &&
            rangeListString.All(dimension => Double.TryParse(dimension, out double _));

        #region IEquatable
        public override bool Equals(object obj) => obj != null && Equals((RangeMicrostation)obj);

        public bool Equals(RangeMicrostation other) =>
            other.LowLeftPoint == LowLeftPoint && other.HighRightPoint == HighRightPoint;


        public static bool operator ==(RangeMicrostation left, RangeMicrostation right) => left.Equals(right);
       
        public static bool operator !=(RangeMicrostation left, RangeMicrostation right) => !(left == right);

        public override int GetHashCode()
        {
            var hashCode = 17;
            hashCode = hashCode * 31 + LowLeftPoint .GetHashCode();
            hashCode = hashCode * 31 + HighRightPoint.GetHashCode();

            return hashCode;
        }
        #endregion
    }
}
