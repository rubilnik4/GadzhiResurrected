using System;

namespace GadzhiMicrostation.Models.Implementations.Coordinates
{
    /// <summary>
    /// Координатная точка
    /// </summary>
    public readonly struct PointMicrostation : IEquatable<PointMicrostation>
    {
        public PointMicrostation(double x, double y)
        {
            X = x;
            Y = y;
            Z = 0;
        }

        public PointMicrostation(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Координата X
        /// </summary>
        public double X { get; }

        /// <summary>
        /// Координата Y
        /// </summary>
        public double Y { get; }

        /// <summary>
        /// Координата Z
        /// </summary>
        public double Z { get; }

        /// <summary>
        /// Операция сложения с другой точкой
        /// </summary>       
        public PointMicrostation Add(PointMicrostation point) => new PointMicrostation(X + point.X, Y + point.Y, Z + point.Z);

        /// <summary>
        /// Операция сложения по оси X
        /// </summary>       
        public PointMicrostation AddX(double xCoordinate) => Add(new PointMicrostation(xCoordinate, 0, 0));

        /// <summary>
        /// Операция сложения по оси Y
        /// </summary>       
        public PointMicrostation AddY(double yCoordinate) => Add(new PointMicrostation(0, yCoordinate, 0));

        /// <summary>
        /// Операция вычитания по оси X
        /// </summary>   
        public PointMicrostation Subtract(PointMicrostation point) => new PointMicrostation(X - point.X, Y - point.Y, Z - point.Z);

        /// <summary>
        /// Операция вычитания по оси Y
        /// </summary>   
        public PointMicrostation SubtractX(double xCoordinate) => Subtract(new PointMicrostation(xCoordinate, 0, 0));

        /// <summary>
        /// Операция вычитания с другой точкой
        /// </summary>   
        public PointMicrostation SubtractY(double yCoordinate) => Subtract(new PointMicrostation(0, yCoordinate, 0));

        /// <summary>
        /// Операция умножения
        /// </summary>       
        public PointMicrostation Multiply(double factor) => new PointMicrostation(X * factor, Y * factor, Z * factor);

        /// <summary>
        /// Число для сравнения координат
        /// </summary>
        private const double TOLERANCE = 0.0001;

        /// <summary>
        /// Сравнение координат
        /// </summary>
        public static bool CompareCoordinate(double first, double second) => Math.Abs(first - second) < TOLERANCE;

        #region IEquatable
        public override bool Equals(object obj) => obj != null && Equals((PointMicrostation)obj);

        public bool Equals(PointMicrostation other) =>
            CompareCoordinate(other.X, X) && CompareCoordinate(other.Y, Y) && CompareCoordinate(other.Z, Z);

        public static bool operator ==(PointMicrostation left, PointMicrostation right) => left.Equals(right);

        public static bool operator !=(PointMicrostation left, PointMicrostation right) => !(left == right);

        public override int GetHashCode()
        {
            var hashCode = 17;
            hashCode = hashCode * 31 + X.GetHashCode();
            hashCode = hashCode * 31 + Y.GetHashCode();
            hashCode = hashCode * 31 + Z.GetHashCode();

            return hashCode;
        }
        #endregion
    }
}
