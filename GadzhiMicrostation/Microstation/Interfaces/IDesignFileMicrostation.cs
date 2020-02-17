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
        /// Загрузился ли файл
        /// </summary>
        bool IsDesingFileValid { get; }

        /// <summary>
        /// Модели и листы в текущем файле
        /// </summary>
        IList<IModelMicrostation> ModelsMicrostation { get; }

        /// <summary>
        /// Найти все штампы во всех моделях и листах
        /// </summary>       
        IList<IStamp> Stamps { get; }
    }
}