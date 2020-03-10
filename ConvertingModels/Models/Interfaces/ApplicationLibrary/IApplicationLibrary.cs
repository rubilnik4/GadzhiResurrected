using GadzhiCommon.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertingModels.Models.Interfaces.ApplicationLibrary
{
    /// <summary>
    /// Класс для работы с приложением Word
    /// </summary>
    public interface IApplicationLibrary: IApplicationLibraryDocument, IApplicationLibraryPrinting
    {    
        /// <summary>
        /// Загрузилась ли оболочка Microstation
        /// </summary>
        bool IsApplicationValid { get; }
    }
}
