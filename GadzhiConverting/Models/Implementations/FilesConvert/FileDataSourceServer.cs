﻿using GadzhiCommon.Enums.FilesConvert;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiConverting.Models.Implementations.FilesConvert
{
    /// <summary>
    /// Отконвертированный файл серверной части
    /// </summary>
    public class FileDataSourceServer
    {   
        public FileDataSourceServer(string filePath, FileExtension fileExtensionType, string paperSize, string printerName)
        {
            if (!String.IsNullOrWhiteSpace(filePath))
            {
                FilePath = filePath;
                FileExtensionType = fileExtensionType;
                PaperSize = paperSize;
                PrinterName = printerName;
            }
            else
            {
                throw new ArgumentNullException(nameof(filePath));
            }
        }

        /// <summary>
        /// Путь файла
        /// </summary>
        public string FilePath { get; }

        /// <summary>
        /// Тип расширения файла
        /// </summary>
        public FileExtension FileExtensionType { get; }

        /// <summary>
        /// Формат печати
        /// </summary>
        public string PaperSize { get; }

        /// <summary>
        /// Имя принтера
        /// </summary>
        public string PrinterName { get; }
    }


}
