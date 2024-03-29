﻿using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.StampPartial;

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
        IResultApplication SaveAs(string filePath);

        /// <summary>
        /// Команда печати PDF
        /// </summary>
        IResultApplication PrintStamp(IStamp stamp, ColorPrintApplication colorPrint, string prefixSearchPaperSize);

        /// <summary>
        /// Экспорт файла в другие форматы
        /// </summary>      
        IResultAppValue<string> Export(string filePath, StampDocumentType stampDocumentType);

        /// <summary>
        /// Подключить дополнительные файлы
        /// </summary>
        void AttachAdditional();

        /// <summary>
        /// Отключить дополнительные файлы
        /// </summary>
        void DetachAdditional();

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
