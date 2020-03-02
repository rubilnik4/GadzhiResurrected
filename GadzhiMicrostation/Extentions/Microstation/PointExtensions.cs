using GadzhiMicrostation.Models.Coordinates;
using MicroStationDGN;

namespace GadzhiMicrostation.Extensions.Microstation
{
    /// <summary>
    /// Преобразование координат
    /// </summary> 
    public static class PointExtensions
    {
        /// <summary>
        /// Преобразование координат в тип Microstation
        /// </summary> 
        public static Point3d ToPoint3d(this PointMicrostation pointMicrostation)
        {
            return new Point3d()
            {
                X = pointMicrostation.X,
                Y = pointMicrostation.Y,
                Z = pointMicrostation.Z,
            };
        }

        /// <summary>
        /// Преобразование координат из типа Microstation
        /// </summary> 
        public static PointMicrostation ToPointMicrostation(this Point3d point3d) =>
             new PointMicrostation(point3d.X, point3d.Y, point3d.Z);

    }
}
