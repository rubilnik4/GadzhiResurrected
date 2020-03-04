using ConvertingModels.Models.Interfaces.FilesConvert;
using GadzhiCommon.Enums.FilesConvert;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertingModels.Models.Implementations.FilesConvert
{
    /// <summary>
    /// Отконвертированный файл серверной части в базовом варианте
    /// </summary>
    public abstract class FileDataSourceServer: IFileDataSourceServer
    {       
        public FileDataSourceServer(string filePath, FileExtention fileExtensionType, string paperSize, string printerName)
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
        public FileExtention FileExtensionType { get; }

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
