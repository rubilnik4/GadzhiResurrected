using GadzhiMicrostation.Models.Implementations.Coordinates;

namespace GadzhiMicrostation.Microstation.Interfaces.Elements
{
    /// <summary>
    /// Текстовый элемент типа Microstation
    /// </summary>
    public interface ITextElementMicrostation : IRangeBaseElementMicrostation<ITextElementMicrostation>
    {
        /// <summary>
        /// Текст элемента
        /// </summary>
        string Text { get; }

        /// <summary>
        /// Переместить элемент
        /// </summary>
        ITextElementMicrostation Move(PointMicrostation offset);

        /// <summary>
        /// Повернуть элемент
        /// </summary>
        ITextElementMicrostation Rotate(PointMicrostation origin, double degree);

        /// <summary>
        /// Масштабировать элемент
        /// </summary>
        ITextElementMicrostation ScaleAll(PointMicrostation origin, PointMicrostation scaleFactor);
    }
}
