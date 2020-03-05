using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiWord.Word.Interfaces.Elements
{
    /// <summary>
    /// Элемент строка
    /// </summary>
    public interface IRowElementWord
    {
        /// <summary>
        /// Список ячеек в строке
        /// </summary>
        IReadOnlyList<ICellElementWord> CellsElementWord { get; }
    }
}
