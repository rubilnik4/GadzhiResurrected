using ConvertingModels.Models.Implementations.FilesConvert;
using ConvertingModels.Models.Interfaces.ApplicationLibrary;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Extentions.StringAdditional;
using GadzhiCommon.Infrastructure.Implementations;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiWord.Models.Implementations.FilesConvert;
using GadzhiWord.Models.Interfaces.FilesConvert;
using GadzhiWord.Word.Interfaces;
using GadzhiWord.Word.Interfaces.ApplicationWordPartial;
using GadzhiWord.Word.Interfaces.DocumentWordPartial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public IDocumentWord OpenDocument(string filePath)
        {
            if (!String.IsNullOrWhiteSpace(filePath))
            {
                Application.Documents.Open(filePath);
            }
            else
            {
                throw new ArgumentNullException(nameof(filePath)));
            }
            return ActiveDocument;
        }

        /// <summary>
        /// Сохранить документ
        /// </summary>
        public IDocumentWord SaveDocument(string filePath)
        {
            if (ActiveDocument.IsDocumentValid)
            {
                if (!String.IsNullOrWhiteSpace(filePath))
                {
                    ActiveDocument.SaveAs(filePath);
                }
                else
                {
                    throw new ArgumentNullException(nameof(filePath)));
                }
            }
            return ActiveDocument;
        }

        /// <summary>
        /// Закрыть файл
        /// </summary>
        public void CloseDocument()
        {
            if (ActiveDocument.IsDocumentValid)
            {
                ActiveDocument.CloseWithSaving();              
            }
        }
    }
}
