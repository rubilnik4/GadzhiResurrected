using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Microstation.Interfaces
{
    /// <summary>
    /// Модель или лист в файле
    /// </summary>
    public interface IModelMicrostation
    {
        /// <summary>
        /// Найти штампы в модели
        /// </summary>    
        IEnumerable<IStamp> FindStamps();
    }
}
