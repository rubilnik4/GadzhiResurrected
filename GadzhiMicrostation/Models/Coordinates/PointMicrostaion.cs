using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Models.Coordinates
{
    /// <summary>
    /// Координатная точка
    /// </summary>
    public class PointMicrostaion
    {
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

        public PointMicrostaion()
            :this (0,0,0)
        {

        }

        public PointMicrostaion(double x, double y)
        {
            X = x;
            Y = y;
            Z = 0;
        }

        public PointMicrostaion(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}
