using GadzhiCommon.Enums.FilesConvert;
using GadzhiModules.Helpers;
using GadzhiModules.Helpers.Converters;
using GadzhiModules.Infrastructure.Implementations.Information;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiModules.Modules.FilesConvertModule.Models.Implementations
{
    /// <summary>
    /// Класс для хранения информации о конвертируемом файле
    /// </summary>
    public class FileData : IEquatable<FileData>
    {
        public FileData(string filePath)
        {
            string fileExtension = FileHelpers.ExtensionWithoutPointFromPath(filePath);
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            if (String.IsNullOrEmpty(fileExtension) || String.IsNullOrEmpty(fileName) || String.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException("Входные параметры FileData имеют пустое значение");
            }

            FileExtension = fileExtension;
            FileName = fileName;
            FilePath = filePath;

            ColorPrint = ColorPrint.BlackAndWhite;
        }

        public FileData(string filePath, ColorPrint colorPrint)
            : this(filePath)
        {
            ColorPrint = colorPrint;
        }

        /// <summary>
        /// Расширение файла
        /// </summary>
        public string FileExtension { get; }

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
        public ColorPrint ColorPrint { get; set; }

        /// <summary>
        /// Статус обработки файла
        /// </summary>
        public StatusProcessing StatusProcessing { get; private set; }

        /// <summary>
        /// Тип ошибки при конвертации файла
        /// </summary>
        public IEnumerable<FileConvertErrorType> FileConvertErrorType { get; private set; }

        /// <summary>
        /// Изменить статус и вид ошибки при необходимости
        /// </summary>
        public void ChangeByFileStatus(FileStatus fileStatus)
        {
            StatusProcessing = fileStatus.StatusProcessing;          ;
        }

        public bool Equals(FileData other)
        {
            return FilePath == other.FilePath;
        }
    }
}
