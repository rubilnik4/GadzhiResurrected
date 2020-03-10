using ConvertingModels.Models.Interfaces.ApplicationLibrary.Application;
using ConvertingModels.Models.Interfaces.ApplicationLibrary.Document;
using GadzhiWord.Word.Implementations.DocumentWordPartial;
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
        /// Текущий документ Word
        /// </summary>
        public IDocumentLibrary ActiveDocument => new DocumentWord(_application.ActiveDocument, this);

        /// <summary>
        /// Загрузился ли файл
        /// </summary>
        public bool IsDocumentValid => ActiveDocument != null;

        /// <summary>
        /// Открыть документ
        /// </summary>
        public IDocumentLibrary OpenDocument(string filePath)
        {
            if (!String.IsNullOrWhiteSpace(filePath))
            {
                Application.Documents.Open(filePath);
            }
            else
            {
                throw new ArgumentNullException(nameof(filePath));
            }
            return ActiveDocument;
        }

        /// <summary>
        /// Сохранить документ
        /// </summary>
        public IDocumentLibrary SaveDocument(string filePath)
        {
            if (ActiveDocument.IsDocumentValid)
            {
                if (!String.IsNullOrWhiteSpace(filePath))
                {
                    ActiveDocument.SaveAs(filePath);
                }
                else
                {
                    throw new ArgumentNullException(nameof(filePath));
                }
            }
            return ActiveDocument;
        }

        /// <summary>
        /// Сохранить и закрыть файл
        /// </summary>
        public void CloseAndSaveDocument()
        {
            if (ActiveDocument.IsDocumentValid)
            {
                ActiveDocument.CloseWithSaving();
            }
        }

        /// <summary>
        /// Закрыть файл
        /// </summary>
        public void CloseDocument()
        {
            if (ActiveDocument.IsDocumentValid)
            {
                ActiveDocument.Close();
            }
        }
    }
}
