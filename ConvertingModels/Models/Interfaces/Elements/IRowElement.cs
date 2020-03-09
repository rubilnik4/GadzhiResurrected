using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiConverting.Word.Interfaces.Elements
{
    /// <summary>
    /// Элемент строка. Базовый вариант
    /// </summary>
    public interface IRowElement
    {
        /// <summary>
        /// Список ячеек в строке
        /// </summary>
        IReadOnlyList<ICellElement> CellsElementWord { get; }
    }
}
