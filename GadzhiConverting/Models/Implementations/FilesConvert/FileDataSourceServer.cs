using GadzhiCommon.Enums.FilesConvert;
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
        public FileDataSourceServer(string filePath, FileExtension fileExtensionType)
        {
            if (!String.IsNullOrWhiteSpace(filePath))
            {
                FilePath = filePath;
                FileExtensionType = fileExtensionType;
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
    }

 
}
