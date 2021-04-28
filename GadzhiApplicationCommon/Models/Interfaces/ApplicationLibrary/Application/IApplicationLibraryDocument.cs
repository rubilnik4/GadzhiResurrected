using GadzhiApplicationCommon.Models.Interfaces.ApplicationLibrary.Document;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiApplicationCommon.Models.Interfaces.ApplicationLibrary.Application
{
    /// <summary>
    /// Подкласс приложения Word для работы с документом
    /// </summary>
    public interface IApplicationLibraryDocument<out TDocumentLibrary>
        where TDocumentLibrary : IDocumentLibrary
    {
        /// <summary>
        /// Открыть документ
        /// </summary>
        IResultAppValue<TDocumentLibrary> OpenDocument(string filePath, string fileName);
            
    }
}
