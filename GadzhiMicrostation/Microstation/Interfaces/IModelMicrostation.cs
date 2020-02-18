using GadzhiMicrostation.Microstation.Implementations.Units;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Microstation.Interfaces
{
    /// <summary>
    /// Модель или лист в файле
    /// </summary>
    public interface IModelMicrostation: IOwnerContainer
    {
        /// <summary>
        /// Коэффициенты преобразования координат в текущие
        /// </summary>
        UnitsMicrostation UnitsMicrostation { get; }

        /// <summary>
        /// Найти штампы в модели
        /// </summary>    
        IEnumerable<IStamp> FindStamps();
    }
}
