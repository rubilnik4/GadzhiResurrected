using GadzhiMicrostation.Models.Implementations.Coordinates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Microstation.Interfaces.Elements
{
    /// <summary>
    /// Элемент линии типа Microstation
    /// </summary>
    public interface ILineElementMicrostation : IElementMicrostation
    {
        /// <summary>
        /// Начальная точка
        /// </summary>
        PointMicrostation StartPoint { get; }

        /// <summary>
        /// Конечная точка
        /// </summary>
        PointMicrostation EndPoint { get; }

        /// <summary>
        /// Переместить элемент
        /// </summary>
        ILineElementMicrostation Move(PointMicrostation offset);

        /// <summary>
        /// Повернуть элемент
        /// </summary>
        ILineElementMicrostation Rotate(PointMicrostation origin, double degree);

        /// <summary>
        /// Масштабировать элемент
        /// </summary>
        ILineElementMicrostation ScaleAll(PointMicrostation origin, PointMicrostation scaleFactor);
    }
}
