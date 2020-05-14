using System.Collections.Generic;
using GadzhiMicrostation.Models.Enums;
using GadzhiMicrostation.Models.Implementations.Coordinates;

namespace GadzhiMicrostation.Microstation.Interfaces.Elements
{
    /// <summary>
    /// Элемент ячейки типа Microstation
    /// </summary>
    public interface ICellElementMicrostation : IRangeBaseElementMicrostation<ICellElementMicrostation>
    {
        /// <summary>
        /// Имя ячейки
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Имя ячейки
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Получить дочерние элементы
        /// </summary>
        IEnumerable<IElementMicrostation> SubElements { get; }

        /// <summary>
        /// Получить дочерние элементы по типу
        /// </summary>
        IEnumerable<IElementMicrostation> GetSubElementsByType(ElementMicrostationType elementMicrostationType);

        /// <summary>
        /// Найти и изменить вложенный в штамп элемент.Только для внешних операций типа Scale, Move
        /// </summary>
        void FindAndChangeSubElement(IElementMicrostation elementMicrostation);

        /// <summary>
        /// Переместить элемент
        /// </summary>
        ICellElementMicrostation Move(PointMicrostation offset);

        /// <summary>
        /// Повернуть элемент
        /// </summary>
        ICellElementMicrostation Rotate(PointMicrostation origin, double degree);

        /// <summary>
        /// Масштабировать элемент
        /// </summary>
        ICellElementMicrostation ScaleAll(PointMicrostation origin, PointMicrostation scaleFactor);
    }
}
