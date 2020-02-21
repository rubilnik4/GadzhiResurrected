using System;

namespace GadzhiMicrostation.Models.Coordinates
{
    /// <summary>
    /// Координатная точка
    /// </summary>
    public struct PointMicrostation : IEquatable<PointMicrostation>
    {
        /// <summary>
        /// Координата X
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Координата Y
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// Координата Z
        /// </summary>
        public double Z { get; set; }

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
        /// Операция сложения с другой точкой
        /// </summary>       
        public PointMicrostation Add(PointMicrostation point) =>
            new PointMicrostation(this.X + point.X, this.Y + point.Y, this.Z + point.Z);

        /// <summary>
        /// Операция вычитания с другой точкой
        /// </summary>   
        public PointMicrostation Subtract(PointMicrostation point) =>
            new PointMicrostation(this.X - point.X, this.Y - point.Y, this.Z - point.Z);

        /// <summary>
        /// Операция умножения
        /// </summary>       
        public PointMicrostation Multiply(double factor) =>
            new PointMicrostation(this.X * factor, this.Y * factor, this.Z * factor);

        public override bool Equals(object obj)
        {
            return Equals((PointMicrostation)obj);
        }

        public bool Equals(PointMicrostation other)
        {           
            return other.X == X && other.Y == Y && other.Z == Z;
        }

        public override int GetHashCode()
        {
            int hashCode = 17;
            hashCode = hashCode * 31 + X.GetHashCode();
            hashCode = hashCode * 31 + Y.GetHashCode();
            hashCode = hashCode * 31 + Z.GetHashCode();

            return hashCode;
        }

        public static bool operator ==(PointMicrostation left, PointMicrostation right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(PointMicrostation left, PointMicrostation right)
        {
            return !(left == right);
        }


    }
}
