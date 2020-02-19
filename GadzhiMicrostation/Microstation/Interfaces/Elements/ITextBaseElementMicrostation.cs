using GadzhiMicrostation.Extensions.Microstation;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Models.Coordinates;
using MicroStationDGN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Microstation.Interfaces.Elements
{
    /// <summary>
    /// Базовый класс для текстовыйх полей
    /// </summary>
    public interface ITextBaseElementMicrostation : IElementMicrostation
    {
        /// <summary>
        /// Ширина ячейки элемента в текущих координатах
        /// </summary>
        double WidthAttributeInUnits { get; }

        /// <summary>
        /// Высота ячейки элемента в текущих координатах
        /// </summary>
        double HeightAttributeInUnits { get; }

        /// <summary>
        /// Расположение элемента в текущих координатах
        /// </summary>
        PointMicrostation OriginPointWithRotationAttributeInUnits { get; }
    }
}
