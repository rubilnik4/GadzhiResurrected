using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Infrastructure.Implementations;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace GadzhiConverting.Models.Implementations.FilesConvert
{
    /// <summary>
    /// Класс для хранения информации о конвертируемом файле
    /// </summary>
    public class FileDataServer : IEquatable<FileDataServer>
    {
        /// <summary>
        /// Тип ошибки при конвертации файла
        /// </summary>
        private readonly List<FileConvertErrorType> _fileConvertErrorTypes;
       
        public FileDataServer(string filePathServer, string filePathClient, ColorPrint colorPrint)
              : this(filePathServer, filePathClient, colorPrint, new List<FileConvertErrorType>())
        {

        }

        public FileDataServer(string filePathServer,
                              string filePathClient,
                              ColorPrint colorPrint,
                              IEnumerable<FileConvertErrorType> fileConvertErrorType)
        {
            string fileType = FileSystemOperations.ExtensionWithoutPointFromPath(filePathServer);
            string fileName = Path.GetFileNameWithoutExtension(filePathServer);

            if (!ValidFileExtentions.DocAndDgnFileTypes.Keys.Contains(fileType))
            {
                throw new KeyNotFoundException(nameof(filePathServer));
            }
            FileExtension = fileType;
            FileName = fileName;
            FilePathServer = filePathServer;
            FilePathClient = filePathClient;
            ColorPrint = colorPrint;
            StatusProcessing = StatusProcessing.InQueue;

            _fileConvertErrorTypes = new List<FileConvertErrorType>(fileConvertErrorType);
            FileDataSourceServer = new List<FileDataSourceServer>();
        }

        /// <summary>
        /// Расширение файла
        /// </summary>
        public string FileExtension { get; }

        /// <summary>
        /// Тип расширения файла
        /// </summary>
        public FileExtension FileExtensionType => 
            ValidFileExtentions.DocAndDgnFileTypes[FileExtension.ToLower(CultureInfo.CurrentCulture)];

        /// <summary>
        /// Имя файла
        /// </summary>
        public string FileName { get; }

        /// <summary>
        /// Путь файла на сервере
        /// </summary>
        public string FilePathServer { get; }

        /// <summary>
        /// Имя файла на клиенте
        /// </summary>
        public string FileNameWithExtensionClient => Path.GetFileName(FilePathClient);

        /// <summary>
        /// Путь файла на клиенте
        /// </summary>
        public string FilePathClient { get; }

        /// <summary>
        /// Цвет печати
        /// </summary>
        public ColorPrint ColorPrint { get; }

        /// <summary>
        /// Статус обработки файла
        /// </summary>
        public StatusProcessing StatusProcessing { get; set; }

        /// <summary>
        /// Завершена ли обработка файла
        /// </summary>
        public bool IsCompleted => StatusProcessing == StatusProcessing.ConvertingComplete;

        /// <summary>
        /// Путь и тип отковенртированных файлов
        /// </summary>
        public IEnumerable<FileDataSourceServer> FileDataSourceServer { get; set; }

        /// <summary>
        /// Тип ошибки при конвертации файла
        /// </summary>
        public IReadOnlyList<FileConvertErrorType> FileConvertErrorTypes => _fileConvertErrorTypes;

        /// <summary>
        /// Количество попыток конвертирования
        /// </summary>      
        public int AttemptingConvertCount { get; set; }

        /// <summary>
        /// Корректна ли модель
        /// </summary>
        public bool IsValid => IsValidByErrorType && IsValidByAttemptingCount;

        /// <summary>
        /// Присутствуют ли ошибки конвертирования
        /// </summary>
        public bool IsValidByErrorType => FileConvertErrorTypes == null || FileConvertErrorTypes.Count == 0;

        /// <summary>
        /// Не превышает ли количество попыток конвертирования
        /// </summary>
        public bool IsValidByAttemptingCount => AttemptingConvertCount <= 2;

        /// <summary>
        /// Добавить ошибку
        /// </summary>
        public void AddFileConvertErrorType(FileConvertErrorType fileConvertErrorType)
        {
            _fileConvertErrorTypes.Add(fileConvertErrorType);
        }

        /// <summary>
        /// Добавить ошибки
        /// </summary>
        public void AddRangeFileConvertErrorType(IEnumerable<FileConvertErrorType> fileConvertErrorTypes)
        {
            _fileConvertErrorTypes.AddRange(fileConvertErrorTypes);
        }

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
