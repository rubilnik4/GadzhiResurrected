using GadzhiCommon.Converters;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Helpers.FileSystem;
using GadzhiCommon.Infrastructure.Implementations;
using GadzhiModules.Modules.FilesConvertModule.Models.Implementations.Information;
using System;
using System.Collections.Generic;
using System.IO;

namespace GadzhiModules.Modules.FilesConvertModule.Models.Implementations
{
    /// <summary>
    /// Класс для хранения информации о конвертируемом файле
    /// </summary>
    public class FileData : IEquatable<FileData>
    {
        public FileData(string filePath)
        {
            string fileExtension = FileSystemOperations.ExtensionWithoutPointFromPath(filePath);
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            if (String.IsNullOrEmpty(fileExtension) || String.IsNullOrEmpty(fileName) || String.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException(nameof(filePath));
            }
            
            if (Enum.TryParse(fileExtension, out FileExtension fileExtensionType))
            {
                FileExtension = fileExtensionType;
            }
            else
            {
                throw new FormatException(nameof(fileExtension));
            }

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
        public FileExtension FileExtension { get; }

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
        /// Статус ошибок
        /// </summary>
        public StatusError StatusError => ConverterErrorType.FileErrorsTypeToStatusError(FileConvertErrorType);

        /// <summary>
        /// Тип ошибки при конвертации файла
        /// </summary>
        public IEnumerable<FileConvertErrorType> FileConvertErrorType { get; private set; }

        /// <summary>
        /// Изменить статус и вид ошибки при необходимости
        /// </summary>
        public void ChangeByFileStatus(FileStatus fileStatus)
        {
            if (fileStatus != null)
            {
                StatusProcessing = fileStatus.StatusProcessing;
                FileConvertErrorType = fileStatus.Errors;
            }
            else
            {
                throw new ArgumentNullException(nameof(fileStatus));
            }
        }

        public override bool Equals(object obj)
        {
            return obj is FileData fileData &&
                   Equals(fileData);
        }

        public bool Equals(FileData other)
        {
            return other?.FilePath == FilePath;
        }

        public override int GetHashCode()
        {
            return 1230029444 + FilePath.GetHashCode();
        }
    }
}
