using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiWord.Word.Interfaces.Elements
{
    /// <summary>
    /// Элемент ячейка
    /// </summary>
    public interface ICellElementWord
    {
        /// <summary>
        /// Текст ячейки
        /// </summary>
        string Text { get; }
    }
}
