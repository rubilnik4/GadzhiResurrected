using GadzhiModules.Helpers.Converters;
using GadzhiModules.Modules.FilesConvertModule.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiModules.Modules.FilesConvertModule.Model
{
    /// <summary>
    /// Класс для хранения информации о конвертируемых файлах
    /// </summary>
    public class FileData
    {     
        public FileData(string fileType, string fileName, string filePath)
        {
            if (String.IsNullOrEmpty(fileType) || String.IsNullOrEmpty(fileName) || String.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException("Входные параметры FileData имеют пустое значение");
            }
            FileType = fileType;
            FileName = fileName;
            FilePath = filePath;
        }

        public FileData(string fileType, string fileName, string filePath, ColorPrint colorPrint)
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

        /// <summary>
        /// Цвет печати строковое значение
        /// </summary>
        public string ColorPrintName => ColorPrintConverter.ConvertColorPrintToString(ColorPrint);
    }
}
