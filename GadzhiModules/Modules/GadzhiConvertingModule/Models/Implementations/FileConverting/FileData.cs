using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Infrastructure.Implementations;
using GadzhiCommon.Infrastructure.Implementations.Converters.Errors;
using GadzhiCommon.Infrastructure.Implementations.Logger;
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
        /// <summary>
        /// Журнал системных сообщений
        /// </summary>
        private readonly ILoggerService _loggerService = LoggerFactory.GetFileLogger();

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

            FileErrors = new List<IErrorCommon>();
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
        public ColorPrint ColorPrint { get; private set; }

        /// <summary>
        /// Статус обработки файла
        /// </summary>
        public StatusProcessing StatusProcessing { get; private set; }

        /// <summary>
        /// Статус ошибок
        /// </summary>
        public StatusError StatusError =>
            ConverterErrorType.ErrorsTypeToStatusError(FileErrors.Select(error => error.FileConvertErrorType));

        /// <summary>
        /// Тип ошибки при конвертации файла
        /// </summary>
        public IReadOnlyCollection<IErrorCommon> FileErrors { get; private set; }

        /// <summary>
        /// Изменить цвет печати
        /// </summary>
        public void SetColorPrint(ColorPrint colorPrint)
        {
            ColorPrint = colorPrint;
            _loggerService.LogByObject(LoggerLevel.Info, LoggerObjectAction.Update, MethodBase.GetCurrentMethod(), ColorPrint, ToString());
        }

        /// <summary>
        /// Изменить статус и вид ошибки при необходимости
        /// </summary>
        public IFileData ChangeByFileStatus(FileStatus fileStatus)
        {
            if (fileStatus == null) throw new ArgumentNullException(nameof(fileStatus));

            SetStatusProcessing(fileStatus.StatusProcessing);
            SetFileErrors(fileStatus.Errors);

            return this;
        }

        /// <summary>
        /// Изменить статус обработки файлов
        /// </summary>
        private void SetStatusProcessing(StatusProcessing statusProcessing)
        {
            StatusProcessing = statusProcessing;
            _loggerService.LogByObject(LoggerLevel.Info, LoggerObjectAction.Update, MethodBase.GetCurrentMethod(), StatusProcessing, ToString());
        }

        /// <summary>
        /// Изменить список ошибок конвертации
        /// </summary>
        private void SetFileErrors(IReadOnlyCollection<IErrorCommon> fileErrors)
        {
            FileErrors = fileErrors;
            _loggerService.LogByObject(LoggerLevel.Info, LoggerObjectAction.Update, MethodBase.GetCurrentMethod(), FileErrors, ToString());
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
