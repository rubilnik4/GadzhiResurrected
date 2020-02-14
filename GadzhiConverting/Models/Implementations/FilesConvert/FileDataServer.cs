using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiConverting.Models.FilesConvert.Implementations
{
    /// <summary>
    /// Класс для хранения информации о конвертируемом файле
    /// </summary>
    public class FileDataServer : IEquatable<FileDataServer>
    {
        /// <summary>
        /// Тип ошибки при конвертации файла
        /// </summary>
        public List<FileConvertErrorType> _fileConvertErrorType;

        public FileDataServer(string filePathServer, string filePathClient, ColorPrint colorPrint)
              : this(filePathServer, filePathClient, colorPrint, new List<FileConvertErrorType>())
        {

        }

        public FileDataServer(string filePathServer, string filePathClient, ColorPrint colorPrint, IEnumerable<FileConvertErrorType> fileConvertErrorType)
        {
            string fileType = FileHelpers.ExtensionWithoutPointFromPath(filePathServer);
            string fileName = Path.GetFileNameWithoutExtension(filePathServer);
           
            FileExtension = fileType;
            FileName = fileName;
            FilePathServer = filePathServer;
            FilePathClient = filePathClient;
            ColorPrint = colorPrint;
            StatusProcessing = StatusProcessing.InQueue;

            _fileConvertErrorType = new List<FileConvertErrorType>();
            _fileConvertErrorType.AddRange(fileConvertErrorType);
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
        /// Тип ошибки при конвертации файла
        /// </summary>
        public IReadOnlyList<FileConvertErrorType> FileConvertErrorType => _fileConvertErrorType;

        /// <summary>
        /// Завершена ли обработка файла
        /// </summary>
        public bool IsCompleted { get; set; }

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
        public bool IsValidByErrorType => FileConvertErrorType == null || FileConvertErrorType.Count == 0;

        /// <summary>
        /// Не превышает ли количество попыток конвертирования
        /// </summary>
        public bool IsValidByAttemptingCount => AttemptingConvertCount <= 2;

        /// <summary>
        /// Добавить ошибку
        /// </summary>
        public void AddFileConvertErrorType(FileConvertErrorType fileConvertErrorType)
        {
            _fileConvertErrorType.Add(fileConvertErrorType);
        }

        public bool Equals(FileDataServer other)
        {
            return FilePathServer == other.FilePathServer;
        }
    }
}
