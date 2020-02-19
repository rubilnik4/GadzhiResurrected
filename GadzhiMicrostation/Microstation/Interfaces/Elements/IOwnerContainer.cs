using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Microstation.Interfaces.Elements
{
    /// <summary>
    /// Родительский элемент
    /// </summary>
    public interface IOwnerContainer
    {
        /// <summary>
        /// Класс для работы с приложением Microstation
        /// </summary>
        IApplicationMicrostation ApplicationMicrostation { get; }

        /// <summary>
        /// Коэффициент преобразования координат в текущие относительно родительского элемента
        /// </summary>
        double UnitScale { get; }
    }
}
