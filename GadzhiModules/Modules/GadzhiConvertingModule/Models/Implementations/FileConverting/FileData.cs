using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Infrastructure.Implementations;
using GadzhiCommon.Infrastructure.Implementations.Converters.Errors;
using GadzhiCommon.Infrastructure.Implementations.FilesConvert;
using GadzhiCommon.Infrastructure.Implementations.Logger;
using GadzhiCommon.Infrastructure.Implementations.Reflection;
using GadzhiCommon.Infrastructure.Interfaces.Logger;
using GadzhiCommon.Models.Enums;
using GadzhiCommon.Models.Interfaces.Errors;
using GadzhiModules.Modules.GadzhiConvertingModule.Models.Implementations.FileConverting.Information;
using GadzhiModules.Modules.GadzhiConvertingModule.Models.Interfaces.FileConverting;

namespace GadzhiModules.Modules.GadzhiConvertingModule.Models.Implementations.FileConverting
{
    /// <summary>
    /// Класс для хранения информации о конвертируемом файле
    /// </summary>
    public class FileData : IFileData, IEquatable<IFileData>, IFormattable
    {
        public FileData(string filePath, ColorPrintType colorPrintType)
        {
            string fileExtension = FilePathOperations.ExtensionWithoutPointFromPath(filePath);
            string fileName = Path.GetFileNameWithoutExtension(filePath);

            if (String.IsNullOrEmpty(fileExtension) || String.IsNullOrEmpty(fileName) || String.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException(nameof(filePath));
            }
            if (!ValidFileExtensions.ContainsInDocAndDgnFileTypes(fileExtension)) throw new KeyNotFoundException(nameof(fileExtension));

            FileExtensionType = ValidFileExtensions.DocAndDgnFileTypeDictionary[fileExtension];
            FileName = fileName;
            FilePath = filePath;
            ColorPrintType = colorPrintType;

            FileErrors = new List<IErrorCommon>();
        }

        /// <summary>
        /// Журнал системных сообщений
        /// </summary>
        private static readonly ILoggerService _loggerService = LoggerFactory.GetFileLogger();

        /// <summary>
        /// Расширение файла
        /// </summary>
        public FileExtensionType FileExtensionType { get; }

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
        public ColorPrintType ColorPrintType { get; private set; }

        /// <summary>
        /// Статус обработки файла
        /// </summary>
        public StatusProcessing StatusProcessing { get; private set; }

        /// <summary>
        /// Статус ошибок
        /// </summary>
        public StatusErrorType StatusErrorType =>
            ConverterErrorType.ErrorsTypeToStatusError(FileErrors.Select(error => error.ErrorConvertingType));

        /// <summary>
        /// Тип ошибки при конвертации файла
        /// </summary>
        public IReadOnlyCollection<IErrorCommon> FileErrors { get; private set; }

        /// <summary>
        /// Изменить цвет печати
        /// </summary>
        public void SetColorPrint(ColorPrintType colorPrintType)
        {
            ColorPrintType = colorPrintType;
            _loggerService.LogByObject(LoggerLevel.Info, LoggerAction.Update, ReflectionInfo.GetMethodBase(this), ColorPrintType, ToString());
        }

        /// <summary>
        /// Изменить статус и вид ошибки при необходимости
        /// </summary>
        public IFileData ChangeByFileStatus(FileStatus fileStatus)
        {
            if (fileStatus == null) throw new ArgumentNullException(nameof(fileStatus));

            StatusProcessing = fileStatus.StatusProcessing;
            FileErrors = fileStatus.Errors;

            if(fileStatus.StatusProcessing == StatusProcessing.End)
            {
                _loggerService.InfoLog($"Converting {nameof(IFileData)} complete: {FilePath}. Has {FileErrors.Count} errors", 
                                       FileErrors.Select(error => error.ToString()));
            }

            return this;
        }
    
        #region IFormattable Support
        public override string ToString() => ToString(String.Empty, CultureInfo.CurrentCulture);

        public string ToString(string format, IFormatProvider formatProvider) => FilePath;
        #endregion

        #region IEquatable
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
        #endregion
    }
}
