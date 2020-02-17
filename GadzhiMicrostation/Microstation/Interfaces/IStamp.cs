using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Microstation.Interfaces
{
    /// <summary>
    /// Штамп
    /// </summary>
    public interface IStamp
    {
        /// <summary>
        /// Вписать текстовые поля в рамки
        /// </summary>
        void CompressFieldsRanges();
    }
}
