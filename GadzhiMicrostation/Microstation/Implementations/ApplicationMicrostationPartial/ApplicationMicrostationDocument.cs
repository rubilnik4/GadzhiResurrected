using GadzhiApplicationCommon.Models.Interfaces.ApplicationLibrary.Application;
using GadzhiApplicationCommon.Models.Interfaces.ApplicationLibrary.Document;
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
        /// Текущий файл Microstation
        /// </summary>
        public IDocumentLibrary ActiveDocument => new DocumentMicrostation(_application.ActiveDesignFile, this);

        /// <summary>
        /// Загрузился ли файл
        /// </summary>
        public bool IsDocumentValid => ActiveDocument != null;

        /// <summary>
        /// Открыть файл
        /// </summary>
        public IDocumentLibrary OpenDocument(string filePath)
        {
            Application.OpenDesignFile(filePath, false);
            return ActiveDocument;
        }

        /// <summary>
        /// Сохранить файл
        /// </summary>
        public IDocumentLibrary SaveDocument(string filePath)
        {
            if (ActiveDocument.IsDocumentValid)
            {
                ActiveDocument.SaveAs(filePath);
            }
            return ActiveDocument;
        }

        /// <summary>
        /// Создать файл типа DWG
        /// </summary>
        public string ExportDocument(string filePath)
        {
            if (ActiveDocument.IsDocumentValid)
            {
                ActiveDocument.Export(filePath);
            }
            return null;
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
