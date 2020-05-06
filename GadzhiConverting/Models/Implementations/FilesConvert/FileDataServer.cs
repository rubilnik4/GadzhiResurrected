using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Infrastructure.Implementations;
using GadzhiCommon.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using GadzhiConverting.Models.Interfaces.FilesConvert;

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
        public const int ATTEMPTING_DEFAULT_COUNT = 1;

        public FileDataServer(IFileDataServer fileDataServer, StatusProcessing statusProcessing, FileConvertErrorType fileConvertErrorType)
         : this(fileDataServer, statusProcessing, new List<FileConvertErrorType>() { fileConvertErrorType }) { }

        public FileDataServer(IFileDataServer fileDataServer, StatusProcessing statusProcessing, IEnumerable<FileConvertErrorType> fileConvertErrorType)
          : this(fileDataServer, statusProcessing, fileDataServer.NonNull().FilesDataSourceServer, fileConvertErrorType) { }
        
        public FileDataServer(IFileDataServer fileDataServer, StatusProcessing statusProcessing,
                              IEnumerable<IFileDataSourceServer> filesDataSourceServer, IEnumerable<FileConvertErrorType> fileConvertErrorType)
        : this(fileDataServer.NonNull().FilePathServer, fileDataServer.NonNull().FilePathClient,
               fileDataServer.NonNull().ColorPrint, statusProcessing,
               fileDataServer.NonNull().AttemptingConvertCount, filesDataSourceServer, fileConvertErrorType)
        { }

        public FileDataServer(string filePathServer, string filePathClient, ColorPrint colorPrint, StatusProcessing statusProcessing,
                              IEnumerable<FileConvertErrorType> fileConvertErrorType)
            : this(filePathServer, filePathClient, colorPrint, statusProcessing, ATTEMPTING_DEFAULT_COUNT, Enumerable.Empty<IFileDataSourceServer>(), fileConvertErrorType)
        { }

        public FileDataServer(string filePathServer, string filePathClient, ColorPrint colorPrint,
                              StatusProcessing statusProcessing, int attemptingConvertCount,
                              IEnumerable<IFileDataSourceServer> filesDataSourceServer, 
                              IEnumerable<FileConvertErrorType> filesConvertErrorType)
        {
            if (String.IsNullOrWhiteSpace(filePathServer)) throw new ArgumentNullException(nameof(filePathServer));
            if (String.IsNullOrWhiteSpace(filePathClient)) throw new ArgumentNullException(nameof(filePathClient));

            string fileType = FileSystemOperations.ExtensionWithoutPointFromPath(filePathServer);
            if (!ValidFileExtensions.ContainsInDocAndDgnFileTypes(fileType)) throw new KeyNotFoundException(nameof(filePathServer));
            if (attemptingConvertCount <= 0) throw new ArgumentOutOfRangeException(nameof(attemptingConvertCount));

            FileExtension = ValidFileExtensions.GetDocAndDgnFileTypes(fileType);
            FilePathServer = filePathServer;
            FilePathClient = filePathClient;
            ColorPrint = colorPrint;
            StatusProcessing = statusProcessing;
            AttemptingConvertCount = attemptingConvertCount;

            FileConvertErrorTypes = filesConvertErrorType  ?? throw new ArgumentNullException(nameof(filesConvertErrorType));
            FilesDataSourceServer = filesDataSourceServer ?? throw new ArgumentNullException(nameof(filesDataSourceServer));
        }

        /// <summary>
        /// Тип расширения файла
        /// </summary>
        public FileExtension FileExtension { get; }

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
        public IEnumerable<IFileDataSourceServer> FilesDataSourceServer { get; }

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
        public IFileDataServer SetAttemptingCount(int attemptingCount) =>
            new FileDataServer(FilePathServer, FilePathClient, ColorPrint, StatusProcessing, 
                               attemptingCount, FilesDataSourceServer, FileConvertErrorTypes);

        #region IEquatable
        public override bool Equals(object obj) => obj is FileDataServer fileDataServer && Equals(fileDataServer);

        public bool Equals(FileDataServer other) => FilePathServer == other?.FilePathServer;

        public override int GetHashCode() => -1576186305 + FilePathServer.GetHashCode();
        #endregion
    }
}
