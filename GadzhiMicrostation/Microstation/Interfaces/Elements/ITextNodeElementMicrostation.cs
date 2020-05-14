using GadzhiMicrostation.Models.Implementations.Coordinates;

namespace GadzhiMicrostation.Microstation.Interfaces.Elements
{ /// <summary>
  /// Текстовое поле типа Microstation
  /// </summary>
    public interface ITextNodeElementMicrostation : IRangeBaseElementMicrostation<ITextNodeElementMicrostation>
    {
        /// <summary>
        /// Переместить элемент
        /// </summary>
        ITextNodeElementMicrostation Move(PointMicrostation offset) ;

        /// <summary>
        /// Повернуть элемент
        /// </summary>
        ITextNodeElementMicrostation Rotate(PointMicrostation origin, double degree);

        /// <summary>
        /// Масштабировать элемент
        /// </summary>
        ITextNodeElementMicrostation ScaleAll(PointMicrostation origin, PointMicrostation scaleFactor);
    }
}
