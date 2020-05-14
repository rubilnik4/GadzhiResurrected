using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GadzhiApplicationCommon.Models.Interfaces.ApplicationLibrary.Document;

namespace GadzhiApplicationCommon.Models.Interfaces.ApplicationLibrary.Application
{
    /// <summary>
    /// Класс для работы с приложениями
    /// </summary>
    public interface IApplicationLibrary<out TDocumentLibrary> : IApplicationLibraryDocument<TDocumentLibrary> , IApplicationLibraryPrinting
        where TDocumentLibrary : IDocumentLibrary
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
