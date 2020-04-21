using ConvertingModels.Models.Interfaces.FilesConvert;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Infrastructure.Implementations;
using GadzhiCommon.Extentions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace GadzhiConverting.Models.Implementations.FilesConvert
{
    /// <summary>
    /// Класс для хранения информации о конвертируемом файле в базовом виде
    /// </summary>
    public class FileDataServer : IFileDataServer, IEquatable<FileDataServer>
    {
        /// <summary>
        /// Стартовое количество попыток конвертирования
        /// </summary>
        public const int AttemptingDefaultCount = 1;

        public FileDataServer(IFileDataServer fileDataServer, StatusProcessing statusProcessing, FileConvertErrorType fileConvertErrorType)
         : this(fileDataServer, statusProcessing, new List<FileConvertErrorType>() { fileConvertErrorType }) { }

        public FileDataServer(IFileDataServer fileDataServer, StatusProcessing statusProcessing, IEnumerable<FileConvertErrorType> fileConvertErrorType)
          : this(fileDataServer, statusProcessing, fileDataServer.NonNull().FileDatasSourceServer, fileConvertErrorType) { }
        public FileDataServer(IFileDataServer fileDataServer, StatusProcessing statusProcessing,
                              IEnumerable<IFileDataSourceServer> fileDatasSourceServer, IEnumerable<FileConvertErrorType> fileConvertErrorType)
        : this(fileDataServer.NonNull().FilePathServer, fileDataServer.NonNull().FilePathClient,
               fileDataServer.NonNull().ColorPrint, statusProcessing,
               fileDataServer.NonNull().AttemptingConvertCount, fileDatasSourceServer, fileConvertErrorType)
        { }

        public FileDataServer(string filePathServer, string filePathClient, ColorPrint colorPrint, StatusProcessing statusProcessing,
                              IEnumerable<FileConvertErrorType> fileConvertErrorType)
            : this(filePathServer, filePathClient, colorPrint, statusProcessing, AttemptingDefaultCount, Enumerable.Empty<IFileDataSourceServer>(), fileConvertErrorType)
        { }

        public FileDataServer(string filePathServer, string filePathClient, ColorPrint colorPrint,
                              StatusProcessing statusProcessing, int attemptingConvertCount,
                              IEnumerable<IFileDataSourceServer> fileDatasSourceServer, IEnumerable<FileConvertErrorType> filesConvertErrorType)
        {
            if (String.IsNullOrWhiteSpace(filePathServer)) throw new ArgumentNullException(nameof(filePathServer));
            if (String.IsNullOrWhiteSpace(filePathClient)) throw new ArgumentNullException(nameof(filePathClient));
            string fileType = FileSystemOperations.ExtensionWithoutPointFromPath(filePathServer);
            if (!ValidFileExtentions.DocAndDgnFileTypes.Keys.Contains(fileType))
            {
                throw new KeyNotFoundException(nameof(filePathServer));
            }
            if (attemptingConvertCount <= 0) throw new ArgumentOutOfRangeException(nameof(attemptingConvertCount));

            FileExtentionType = ValidFileExtentions.DocAndDgnFileTypes[fileType.ToLower(CultureInfo.CurrentCulture)];
            FilePathServer = filePathServer;
            FilePathClient = filePathClient;
            ColorPrint = colorPrint;
            StatusProcessing = statusProcessing;
            AttemptingConvertCount = attemptingConvertCount;

            FileConvertErrorTypes = filesConvertErrorType;
            FileDatasSourceServer = fileDatasSourceServer;
        }

        /// <summary>
        /// Тип расширения файла
        /// </summary>
        public FileExtention FileExtentionType { get; }

        /// <summary>
        /// Путь файла на сервере
        /// </summary>
        public string FilePathServer { get; }

        /// <summary>
        /// Путь файла на клиенте
        /// </summary>
        public string FilePathClient { get; }

        /// <summary>
        /// Имя файла на клиенте
        /// </summary>
        public string FileNameClient => Path.GetFileName(FilePathClient);

        /// <summary>
        /// Имя файла без расширения на клиенте
        /// </summary>
        public string FileNameWithoutExtensionClient => Path.GetFileNameWithoutExtension(FilePathClient);

        /// <summary>
        /// Цвет печати
        /// </summary>
        public ColorPrint ColorPrint { get; }

        /// <summary>
        /// Путь и тип отконвертированных файлов
        /// </summary>
        public IEnumerable<IFileDataSourceServer> FileDatasSourceServer { get; }

        /// <summary>
        /// Тип ошибки при конвертации файла
        /// </summary>
        public IEnumerable<FileConvertErrorType> FileConvertErrorTypes { get; }

        /// <summary>
        /// Статус обработки файла
        /// </summary>
        public StatusProcessing StatusProcessing { get; }

        /// <summary>
        /// Завершена ли обработка файла
        /// </summary>
        public bool IsCompleted => CheckStatusProcessing.CompletedStatusProcessingServer.Contains(StatusProcessing);

        /// <summary>
        /// Количество попыток конвертирования
        /// </summary>      
        public int AttemptingConvertCount { get; }

        /// <summary>
        /// Корректна ли модель
        /// </summary>
        public bool IsValid => IsValidByErrorType && IsValidByAttemptingCount;

        /// <summary>
        /// Присутствуют ли ошибки конвертирования
        /// </summary>
        public bool IsValidByErrorType => FileConvertErrorTypes.Any();

        /// <summary>
        /// Не превышает ли количество попыток конвертирования
        /// </summary>
        public bool IsValidByAttemptingCount => AttemptingConvertCount <= 2;

        /// <summary>
        /// Установить количество попыток конвертирования
        /// </summary>       
        public IFileDataServer SetAttemtingCount(int attemptingCount) =>
            new FileDataServer(FilePathServer, FilePathClient, ColorPrint, StatusProcessing, 
                               attemptingCount, FileDatasSourceServer, FileConvertErrorTypes);

        public override bool Equals(object obj)
        {
            return obj is FileDataServer fileDataServer &&
                   Equals(fileDataServer);
        }

        public bool Equals(FileDataServer other)
        {
            return FilePathServer == other?.FilePathServer;
        }

        public override int GetHashCode()
        {
            return -1576186305 + FilePathServer.GetHashCode();
        }
    }
}
