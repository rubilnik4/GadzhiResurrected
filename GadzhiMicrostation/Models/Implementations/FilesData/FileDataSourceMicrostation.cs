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
        {
            if (!String.IsNullOrEmpty(filePath))
            {
                FilePath = filePath;
                FileExtentionMicrostation = fileExtentionMicrostation;
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
    }
}
