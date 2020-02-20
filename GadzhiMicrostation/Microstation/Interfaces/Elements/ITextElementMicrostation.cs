using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Microstation.Interfaces.Elements
{
    /// <summary>
    /// Текстовый элемент типа Microstation
    /// </summary>
    public interface ITextElementMicrostation : IRangeBaseElementMicrostation
    {
        /// <summary>
        /// Текст элемента
        /// </summary>
        string Text { get; }

        /// <summary>
        /// Вписать текстовый элемент в рамку
        /// </summary>
        bool CompressRange();
    }
}
