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
using GadzhiWord.Word.Interfaces;

namespace GadzhiWord.Word.Implementations.ApplicationWordPartial
{
    /// <summary>
    /// Подкласс приложения Word для работы с документом
    /// </summary>
    public partial class ApplicationWord : IApplicationLibraryDocument<IDocumentWord>
    {
        /// <summary>
        /// Открыть документ
        /// </summary>
        public IResultAppValue<IDocumentWord> OpenDocument(string filePath) =>
           new ResultAppValue<IDocumentWord>(new DocumentWord(_application.ActiveDocument, this),
                                             new ErrorApplication(ErrorApplicationType.FileNotOpen, "Документ Word не создан"));
    }
}
