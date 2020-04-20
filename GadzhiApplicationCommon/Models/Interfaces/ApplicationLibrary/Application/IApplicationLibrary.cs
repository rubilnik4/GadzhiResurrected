using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiApplicationCommon.Models.Interfaces.ApplicationLibrary.Application
{
    /// <summary>
    /// Класс для работы с приложениями
    /// </summary>
    public interface IApplicationLibrary: IApplicationLibraryDocument, IApplicationLibraryPrinting
    {  
        /// <summary>
        /// Загрузилась ли оболочка приложения
        /// </summary>
        bool IsApplicationValid { get; }

        /// <summary>
        /// Закрыть приложение
        /// </summary>
        void CloseApplication();
    }
}
