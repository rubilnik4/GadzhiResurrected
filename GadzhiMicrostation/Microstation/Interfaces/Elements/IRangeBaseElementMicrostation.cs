using GadzhiMicrostation.Models.Implementations.Coordinates;

namespace GadzhiMicrostation.Microstation.Interfaces.Elements
{
    /// <summary>
    /// Базовый класс для элементов находящихся в рамке
    /// </summary>
    public interface IRangeBaseElementMicrostation<out TElementRange >: IElementMicrostation
        where TElementRange: IElementMicrostation
    {
        /// <summary>
        /// Необходимо ли сжатие в рамке
        /// </summary>
        bool IsNeedCompress { get; }

        /// <summary>
        /// Вертикальное расположение
        /// </summary>
        bool IsVertical { get; }

        /// <summary>
        /// Координаты текстового элемента
        /// </summary>
        PointMicrostation Origin { get; }

        /// <summary>
        /// Размеры ячейки элемента в текущих координатах
        /// </summary>
        RangeMicrostation Range { get; }

        /// <summary>
        /// Размеры ячейки элемента в стандартно заданных координатах
        /// </summary>
        RangeMicrostation RangeAttributeInUnits { get; }

        /// <summary>
        /// Вписать элемент в рамку
        /// </summary>
        bool CompressRange();

        /// <summary>
        /// Копировать элемент
        /// </summary>
        TElementRange Clone();
    }
}
