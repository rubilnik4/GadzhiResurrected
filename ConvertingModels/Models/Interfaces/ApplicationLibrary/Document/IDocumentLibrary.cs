using GadzhiCommon.Enums.FilesConvert;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertingModels.Models.Interfaces.ApplicationLibrary.Document
{
    /// <summary>
    /// Документ Word
    /// </summary>
    public interface IDocumentLibrary: IDocumentLibraryElements
    {
        /// <summary>
        /// Путь к файлу
        /// </summary>
        string FullName { get; }

        /// <summary>
        /// Формат
        /// </summary>
        string PaperSize { get; }

        /// <summary>
        /// Загрузился ли файл
        /// </summary>
        bool IsDocumentValid { get; }

        /// <summary>
        /// Сохранить файл
        /// </summary>
        void Save();

        /// <summary>
        /// Сохранить файл
        /// </summary>
        void SaveAs(string filePath);

        /// <summary>
        /// Закрыть файл файл
        /// </summary>
        void Close() ;

        /// <summary>
        /// Закрыть файл файл
        /// </summary>
        void CloseWithSaving();       
    }
}
