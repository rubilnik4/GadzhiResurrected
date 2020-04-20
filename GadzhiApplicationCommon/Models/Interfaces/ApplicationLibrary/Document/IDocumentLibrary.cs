using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiApplicationCommon.Models.Interfaces.ApplicationLibrary.Document
{
    /// <summary>
    /// Документ приложения
    /// </summary>
    public interface IDocumentLibrary: IDocumentLibraryElements
    {
        /// <summary>
        /// Путь к файлу
        /// </summary>
        string FullName { get; }       

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
        /// Экспорт файла в другие форматы
        /// </summary>      
        string Export(string filePath);

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
