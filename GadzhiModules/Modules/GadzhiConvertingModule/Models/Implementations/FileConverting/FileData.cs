using System;
using System.Collections.Generic;
using System.IO;
using GadzhiCommon.Converters;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Infrastructure.Implementations;
using GadzhiModules.Modules.GadzhiConvertingModule.Models.Implementations.FileConverting.Information;
using GadzhiModules.Modules.GadzhiConvertingModule.Models.Interfaces.FileConverting;

namespace GadzhiModules.Modules.GadzhiConvertingModule.Models.Implementations.FileConverting
{
    /// <summary>
    /// Класс для хранения информации о конвертируемом файле
    /// </summary>
    public class FileData : IFileData, IEquatable<IFileData>
    {
        public FileData(string filePath)
            : this(filePath, ColorPrint.BlackAndWhite)
        { }

        public FileData(string filePath, ColorPrint colorPrint)
        {
            string fileExtension = FileSystemOperations.ExtensionWithoutPointFromPath(filePath);
            string fileName = Path.GetFileNameWithoutExtension(filePath);

            if (String.IsNullOrEmpty(fileExtension) || String.IsNullOrEmpty(fileName) || String.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException(nameof(filePath));
            }
            if (!ValidFileExtensions.ContainsInDocAndDgnFileTypes(fileExtension)) throw new KeyNotFoundException(nameof(fileExtension));

            FileExtension = ValidFileExtensions.DocAndDgnFileTypeDictionary[fileExtension];
            FileName = fileName;
            FilePath = filePath;
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
        public IReadOnlyCollection<FileConvertErrorType> FileConvertErrorType { get; private set; }

        /// <summary>
        /// Изменить статус и вид ошибки при необходимости
        /// </summary>
        public IFileData ChangeByFileStatus(FileStatus fileStatus)
        {
            if (fileStatus == null) throw new ArgumentNullException(nameof(fileStatus));

            StatusProcessing = fileStatus.StatusProcessing;
            FileConvertErrorType = fileStatus.Errors;
            return this;
        }

        public override bool Equals(object obj)
        {
            return obj is IFileData fileData &&
                   Equals(fileData);
        }

        public bool Equals(IFileData other)
        {
            return other?.FilePath == FilePath;
        }

        public override int GetHashCode()
        {
            return 1230029444 + FilePath.GetHashCode();
        }
    }
}
