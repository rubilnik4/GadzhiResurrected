using System.Collections.Generic;

namespace GadzhiWord.Word.Interfaces.Word.Elements
{
    /// <summary>
    /// Элемент строка. Базовый вариант
    /// </summary>
    public interface IRowElementWord
    {
        /// <summary>
        /// Список ячеек в строке
        /// </summary>
        IReadOnlyList<ICellElementWord> CellsElement { get; }

        /// <summary>
        /// Индекс строки
        /// </summary>
        int Index { get; }
    }
}
