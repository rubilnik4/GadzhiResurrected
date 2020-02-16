using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Extensions.Microstation
{
    /// <summary>
    /// Методы расширения для ячеек Microstation
    /// </summary>
    public static class CellElementExtations
    {
        public static int WordCount(this String str)
        {
            return str.Split(new char[] { ' ', '.', '?' },
                             StringSplitOptions.RemoveEmptyEntries).Length;
        }
    }
}
