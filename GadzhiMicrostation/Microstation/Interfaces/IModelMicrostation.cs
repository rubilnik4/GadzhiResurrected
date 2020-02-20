using GadzhiMicrostation.Microstation.Implementations.Units;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Microstation.Interfaces.StampPartial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Microstation.Interfaces
{
    /// <summary>
    /// Модель или лист в файле
    /// </summary>
    public interface IModelMicrostation: IOwnerContainerMicrostation
    {
        /// <summary>
        /// Порядковый идентефикационный номер
        /// </summary>
        string IdName { get; }

        /// <summary>
        /// Найти штампы в модели
        /// </summary>    
        IEnumerable<IStamp> FindStamps();
    }
}
