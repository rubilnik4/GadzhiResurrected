using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Infrastructure.Implementations;
using GadzhiCommon.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GadzhiCommon.Models.Interfaces.Errors;
using GadzhiConverting.Models.Interfaces.FilesConvert;

namespace GadzhiConverting.Models.Implementations.FilesConvert
{
    /// <summary>
    /// Класс для хранения информации о конвертируемом файле в базовом виде
    /// </summary>
    public class FileDataServer : FilePath, IFileDataServer
    {
        /// <summary>
        /// Стартовое количество попыток конвертирования
        /// </summary>
        public const int ATTEMPTING_DEFAULT_COUNT = 1;

        public FileDataServer(IFileDataServer fileDataServer, StatusProcessing statusProcessing, IErrorCommon fileErrors)
         : this(fileDataServer, statusProcessing, new List<IErrorCommon>() { fileErrors }) { }

        public FileDataServer(IFileDataServer fileDataServer, StatusProcessing statusProcessing, IEnumerable<IErrorCommon> fileErrors)
          : this(fileDataServer, statusProcessing, fileDataServer.NonNull().FilesDataSourceServer, fileErrors) { }
        
        public FileDataServer(IFileDataServer fileDataServer, StatusProcessing statusProcessing,
                              IEnumerable<IFileDataSourceServer> filesDataSourceServer, IEnumerable<IErrorCommon> fileErrors)
        : this(fileDataServer.NonNull().FilePathServer, fileDataServer.NonNull().FilePathClient,
               fileDataServer.NonNull().ColorPrint, statusProcessing,
               fileDataServer.NonNull().AttemptingConvertCount, filesDataSourceServer, fileErrors)
        { }

        public FileDataServer(string filePathServer, string filePathClient, ColorPrint colorPrint, StatusProcessing statusProcessing,
                              IEnumerable<IErrorCommon> fileErrors)
            : this(filePathServer, filePathClient, colorPrint, statusProcessing, ATTEMPTING_DEFAULT_COUNT,
                   Enumerable.Empty<IFileDataSourceServer>(), fileErrors)
        { }

        public FileDataServer(string filePathServer, string filePathClient, ColorPrint colorPrint,
                              StatusProcessing statusProcessing, int attemptingConvertCount,
                              IEnumerable<IFileDataSourceServer> filesDataSourceServer, 
                              IEnumerable<IErrorCommon> fileErrors)
            :base(filePathServer, filePathClient)
        {
            ColorPrint = colorPrint;
            StatusProcessing = statusProcessing;
            AttemptingConvertCount = attemptingConvertCount;

            FileErrors = fileErrors?.ToList().AsReadOnly() ?? throw new ArgumentNullException(nameof(fileErrors));
            FilesDataSourceServer = filesDataSourceServer?.ToList().AsReadOnly() ?? throw new ArgumentNullException(nameof(filesDataSourceServer));
        }

        /// <summary>
        /// Цвет печати
        /// </summary>
        public ColorPrint ColorPrint { get; }

        /// <summary>
        /// Путь и тип отконвертированных файлов
        /// </summary>
        public IReadOnlyCollection<IFileDataSourceServer> FilesDataSourceServer { get; }

        /// <summary>
        /// Тип ошибки при конвертации файла
        /// </summary>
        public IReadOnlyCollection<IErrorCommon> FileErrors { get; }

        /// <summary>
        /// Статус обработки файла
        /// </summary>
        public StatusProcessing StatusProcessing { get; }

        /// <summary>
        /// Завершена ли обработка файла
        /// </summary>
        public bool IsCompleted => CheckStatusProcessing.CompletedStatusProcessing.Contains(StatusProcessing);

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
        public bool IsValidByErrorType => FileErrors.Any();

        /// <summary>
        /// Не превышает ли количество попыток конвертирования
        /// </summary>
        public bool IsValidByAttemptingCount => AttemptingConvertCount <= 2;

        /// <summary>
        /// Установить количество попыток конвертирования
        /// </summary>       
        public IFileDataServer SetAttemptingCount(int attemptingCount) =>
            new FileDataServer(FilePathServer, FileNameClient, ColorPrint, StatusProcessing, 
                               attemptingCount, FilesDataSourceServer, FileErrors);
    }
}
