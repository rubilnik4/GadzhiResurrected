using GadzhiApplicationCommon.Models.Interfaces.ApplicationLibrary.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiApplicationCommon.Models.Interfaces.ApplicationLibrary.Application
{
    /// <summary>
    /// Подкласс приложения Word для работы с документом
    /// </summary>
    public interface IApplicationLibraryDocument
    {
        /// <summary>
        /// Текущий документ Word
        /// </summary>
        IDocumentLibrary ActiveDocument { get; }

        /// <summary>
        /// Загрузился ли файл
        /// </summary>
        bool IsDocumentValid { get; }

        /// <summary>
        /// Открыть документ
        /// </summary>
        IDocumentLibrary OpenDocument(string filePath);

        /// <summary>
        /// Сохранить документ
        /// </summary>
        IDocumentLibrary SaveDocument(string filePath);
       
        /// <summary>
        /// Сохранить и закрыть файл
        /// </summary>
        void CloseAndSaveDocument();

        /// <summary>
        /// Закрыть файл
        /// </summary>
        void CloseDocument();       
    }

}
