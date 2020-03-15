using System.Collections.Generic;

namespace GadzhiMicrostation.Microstation.Interfaces.Elements
{
    /// <summary>
    /// Элемент ячейки типа Microstation
    /// </summary>
    public interface ICellElementMicrostation : IRangeBaseElementMicrostation
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
    }
}
