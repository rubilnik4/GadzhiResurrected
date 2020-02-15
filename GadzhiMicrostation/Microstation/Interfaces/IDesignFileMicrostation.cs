using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Microstation.Interfaces
{
    /// <summary>
    /// Текущий файл Microstation
    /// </summary>
    public interface IDesignFileMicrostation
    {
        /// <summary>
        /// Модели и листы в текущем файле
        /// </summary>
        IEnumerable<IModelMicrostation> ModelsMicrostation { get; }
    }
}
