using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertingModels.Models.Interfaces.ApplicationLibrary
{
    /// <summary>
    /// Подкласс приложения Word для работы с документом
    /// </summary>
    public interface IApplicationLibraryDocument
    {
        /// <summary>
        /// Открыть документ
        /// </summary>
        IDocumentLibrary OpenDocument(string filePath);

        /// <summary>
        /// Сохранить документ
        /// </summary>
        IDocumentLibrary SaveDocument(string filePath);

        /// <summary>
        /// Закрыть файл
        /// </summary>
        void CloseDocument();
    }
}
