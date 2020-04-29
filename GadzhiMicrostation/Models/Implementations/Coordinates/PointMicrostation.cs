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
        public PointMicrostation Add(PointMicrostation point) =>
            new PointMicrostation(this.X + point.X, this.Y + point.Y, this.Z + point.Z);

        /// <summary>
        /// Операция сложения по оси X
        /// </summary>       
        public PointMicrostation AddX(double xCoorditate) => Add(new PointMicrostation(xCoorditate, 0, 0));

        /// <summary>
        /// Операция сложения по оси Y
        /// </summary>       
        public PointMicrostation AddY(double yCoorditate) => Add(new PointMicrostation(0, yCoorditate, 0));

        /// <summary>
        /// Операция вычитания по оси X
        /// </summary>   
        public PointMicrostation Subtract(PointMicrostation point) =>
            new PointMicrostation(this.X - point.X, this.Y - point.Y, this.Z - point.Z);

        /// <summary>
        /// Операция вычитания по оси Y
        /// </summary>   
        public PointMicrostation SubtractX(double xCoorditate) => Subtract(new PointMicrostation(xCoorditate, 0, 0));

        /// <summary>
        /// Операция вычитания с другой точкой
        /// </summary>   
        public PointMicrostation SubtractY(double yCoorditate) => Subtract(new PointMicrostation(0, yCoorditate, 0));

        /// <summary>
        /// Операция умножения
        /// </summary>       
        public PointMicrostation Multiply(double factor) =>
            new PointMicrostation(this.X * factor, this.Y * factor, this.Z * factor);

        /// <summary>
        /// Число для сравнения координат
        /// </summary>
        private const double TOLERANCE = 0.0001;

        /// <summary>
        /// Сравнение координат
        /// </summary>
        private static bool CompareCoordinate(double first, double second) => Math.Abs(first - second) < TOLERANCE;

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
