using GadzhiMicrostation.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Models.Implementations.FilesData
{
    /// <summary>
    /// Отконвертированный файл модуля Microstation
    /// </summary>
    public class FileDataSourceMicrostation
    {
        public FileDataSourceMicrostation(string filePath, FileExtentionMicrostation fileExtentionMicrostation)
          : this(filePath, fileExtentionMicrostation, "-", "-")
        {

        }
       
        public FileDataSourceMicrostation(string filePath, FileExtentionMicrostation fileExtentionMicrostation, string paperSize, string printerName)
        {
            if (!String.IsNullOrEmpty(filePath))
            {
                FilePath = filePath;
                FileExtentionMicrostation = fileExtentionMicrostation;
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
        public FileExtentionMicrostation FileExtentionMicrostation { get; }

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
