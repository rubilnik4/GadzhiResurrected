using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Microstation.Interfaces.Elements
{ /// <summary>
  /// Текстовое поле типа Microstation
  /// </summary>
    public interface ITextNodeElementMicrostation : ITextBaseElementMicrostation
    {
        /// <summary>
        /// Вписать текстовое поле в рамку
        /// </summary>
        bool CompressRange();
    }
}
