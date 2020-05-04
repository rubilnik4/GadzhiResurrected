using GadzhiApplicationCommon.Functional;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GadzhiApplicationCommon.Models.Enums;

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
        /// Команда печати PDF
        /// </summary>
        IResultApplication PrintStamp(IStamp stamp, ColorPrintApplication colorPrint, string prefixSearchPaperSize);

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

        /// <summary>
        /// Закрыть приложение
        /// </summary>
        void CloseApplication();
    }
}
