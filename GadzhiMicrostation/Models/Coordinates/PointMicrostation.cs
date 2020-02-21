namespace GadzhiMicrostation.Models.Coordinates
{
    /// <summary>
    /// Координатная точка
    /// </summary>
    public class PointMicrostation
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

        public PointMicrostation()
            : this(0, 0, 0)
        {

        }

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
        public static PointMicrostation operator +(PointMicrostation pointMicrostation, PointMicrostation addition) =>
            new PointMicrostation(pointMicrostation.X + addition.X, pointMicrostation.Y + addition.Y, pointMicrostation.Z + addition.Z);

        /// <summary>
        /// Операция вычитания с другой точкой
        /// </summary>       
        public static PointMicrostation operator -(PointMicrostation pointMicrostation, PointMicrostation addition) =>
            new PointMicrostation(pointMicrostation.X - addition.X, pointMicrostation.Y - addition.Y, pointMicrostation.Z - addition.Z);

        /// <summary>
        /// Операция умножения
        /// </summary>       
        public static PointMicrostation operator *(PointMicrostation pointMicrostation, double factor) =>
            new PointMicrostation(pointMicrostation.X * factor, pointMicrostation.Y * factor, pointMicrostation.Z * factor);
    }
}
