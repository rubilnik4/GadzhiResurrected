using System.Collections.Generic;

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
        /// Найти и изменить вложенный в штамп элемент.Только для внешних операций типа Scale, Move
        /// </summary>
        void FindAndChangeSubElement(IElementMicrostation elementMicrostation);
    }
}
