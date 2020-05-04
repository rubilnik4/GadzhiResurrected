using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Interfaces.ApplicationLibrary.Application;
using GadzhiApplicationCommon.Models.Interfaces.ApplicationLibrary.Document;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiWord.Word.Implementations.DocumentWordPartial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GadzhiApplicationCommon.Models.Enums;

namespace GadzhiWord.Word.Implementations.ApplicationWordPartial
{
    /// <summary>
    /// Подкласс приложения Word для работы с документом
    /// </summary>
    public partial class ApplicationWord : IApplicationLibraryDocument
    {
        /// <summary>
        /// Открыть документ
        /// </summary>
        public IResultAppValue<IDocumentLibrary> OpenDocument(string filePath) =>
           new ResultAppValue<IDocumentLibrary>(new DocumentWord(_application.ActiveDocument, this),
                                                new ErrorApplication(ErrorApplicationType.FileNotOpen, "Документ Word не создан"));
    }
}
