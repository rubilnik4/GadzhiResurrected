using GadzhiMicrostation.Models.Coordinates;

namespace GadzhiMicrostation.Microstation.Interfaces.Elements
{
    /// <summary>
    /// Базовый класс для элементов находящихся в рамке
    /// </summary>
    public interface IRangeBaseElementMicrostation : IElementMicrostation
    {
        /// <summary>
        /// Необходимо ли сжатие в рамке
        /// </summary>
        bool IsNeedCompress { get; set; }

        /// <summary>
        /// Вертикальное расположение
        /// </summary>
        bool IsVertical { get; set; }

        /// <summary>
        /// Координаты текстового элемента
        /// </summary>
        PointMicrostation Origin { get; }

        /// <summary>
        /// Нижняя левая точка
        /// </summary>
        PointMicrostation LowLeftPoint { get; }

        /// <summary>
        /// Ширина ячейки элемента в текущих координатах
        /// </summary>
        double WidthAttributeInUnits { get; }

        /// <summary>
        /// Высота ячейки элемента в текущих координатах
        /// </summary>
        double HeightAttributeInUnits { get; }

        /// <summary>
        /// Расположение элемента в текущих координатах. Левая нижняя точка
        /// </summary>
        PointMicrostation OriginPointWithRotationAttributeInUnits { get; }

        /// <summary>
        /// Ширина элемента
        /// </summary>
        double Width { get; }

        /// <summary>
        /// Высота элемента
        /// </summary>
        double Height { get; }

        /// <summary>
        /// Вписать элемент в рамку
        /// </summary>
        bool CompressRange();
    }
}
