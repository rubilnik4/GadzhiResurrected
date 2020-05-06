using GadzhiApplicationCommon.Extensions.Functional;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Interfaces.ApplicationLibrary.Application;
using GadzhiApplicationCommon.Models.Interfaces.ApplicationLibrary.Document;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using MicroStationDGN;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using GadzhiApplicationCommon.Models.Enums;

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
            Application.OpenDesignFile(filePath, false).
            Map(openDocument => new ResultAppValue<IDocumentLibrary>(new DocumentMicrostationPartial.DocumentMicrostation(_application, this),
                                             new ErrorApplication(ErrorApplicationType.FileNotOpen, "Документ Microstation не создан")));
    }
}
