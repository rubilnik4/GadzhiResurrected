using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Interfaces.ApplicationLibrary.Application;
using GadzhiApplicationCommon.Models.Interfaces.ApplicationLibrary.Document;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiMicrostation.Models.Enums;
using MicroStationDGN;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Microstation.Implementations.ApplicationMicrostationPartial
{
    /// <summary>
    /// Подкласс приложения Microstation для работы с файлом
    /// </summary>
    public partial class ApplicationMicrostation : IApplicationLibraryDocument
    {        
        /// <summary>
        /// Открыть файл
        /// </summary>
        public IResultAppValue<IDocumentLibrary> OpenDocument(string filePath) =>
            new ResultAppValue<IDocumentLibrary>(new DocumentMicrostation(Application.OpenDesignFile(filePath, false), this),
                                              new ErrorApplication(ErrorApplicationType.FileNotOpen, "Документ Microstation не создан"));       
    }
}
