using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiWord.Word.Interfaces.Elements
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
