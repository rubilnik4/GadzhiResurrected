using ConvertingModels.Models.Interfaces.ApplicationLibrary.Document;
using GadzhiCommon.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertingModels.Models.Interfaces.ApplicationLibrary.Application
{
    /// <summary>
    /// Класс для работы с приложениями
    /// </summary>
    public interface IApplicationLibrary: IApplicationLibraryDocument, IApplicationLibraryPrinting, IApplicationLibraryStamp
    {  
        /// <summary>
        /// Загрузилась ли оболочка приложения
        /// </summary>
        bool IsApplicationValid { get; }
    }
}
