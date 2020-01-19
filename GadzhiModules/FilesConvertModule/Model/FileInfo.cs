using GadzhiModules.FilesConvertModule.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiModules.FilesConvertModule.Model
{
    /// <summary>
    /// Класс для хранения информации о конвертируемых файлах
    /// </summary>
    public class FileInfo
    {
        public FileInfo(string fileType, string fileName, string filePath)
        {
            FileType = fileType;
            FileName = fileName;
            FilePath = filePath;
        }

        public FileInfo(string fileType, string fileName, string filePath, ColorPrint colorPrint)
            : this(fileType, fileName, filePath)
        {
            ColorPrint = colorPrint;
        }

        /// <summary>
        /// Расширение файла
        /// </summary>
        public string FileType { get; }

        /// <summary>
        /// Имя файла
        /// </summary>
        public string FileName { get; }

        /// <summary>
        /// Путь файла
        /// </summary>
        public string FilePath { get; }

        /// <summary>
        /// Цвет печати
        /// </summary>
        public ColorPrint ColorPrint { get; set; } = ColorPrint.BlackAndWhite;
    }
}
