using GadzhiApplicationCommon.Models.Interfaces.ApplicationLibrary.Application;
using GadzhiApplicationCommon.Models.Interfaces.ApplicationLibrary.Document;
using GadzhiMicrostation.Extentions.StringAdditional;
using GadzhiMicrostation.Infrastructure.Implementations;
using GadzhiMicrostation.Microstation.Interfaces;
using GadzhiMicrostation.Microstation.Interfaces.ApplicationMicrostationPartial;
using GadzhiMicrostation.Models.Enums;
using GadzhiMicrostation.Models.Implementations;
using GadzhiMicrostation.Models.Implementations.FilesData;
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
            if (!String.IsNullOrEmpty(filePath))
            {
                Application.OpenDesignFile(filePath, false);
            }
            else
            {
                throw new ArgumentNullException(nameof(filePath));
            }
            return ActiveDocument;
        }

        /// <summary>
        /// Сохранить файл
        /// </summary>
        public IDocumentLibrary SaveDocument(string filePath)
        {
            if (ActiveDocument.IsDocumentValid)
            {
                if (!String.IsNullOrEmpty(filePath))
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
        /// Создать файл типа DWG
        /// </summary>
        public string ExportDocument(string filePath)
        {
            if (ActiveDocument.IsDocumentValid)
            {
                if (!String.IsNullOrEmpty(filePath))
                {
                    ActiveDocument.Export(filePath);
                }
                else
                {
                    throw new ArgumentNullException(nameof(filePath));
                }
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
