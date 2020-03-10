using ConvertingModels.Models.Interfaces.FilesConvert;
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
    /// Класс для хранения информации о конвертируемом файле в базовом виде
    /// </summary>
    public abstract class FileDataServer : IFileDataServer, IEquatable<FileDataServer>
    {
        /// <summary>
        /// Пути отконвертированных файлов
        /// </summary>
        protected List<IFileDataSourceServer> FileDatasSourceServerBase { get; }

        /// <summary>
        /// Тип ошибки при конвертации файла
        /// </summary>
        protected List<FileConvertErrorType> FileConvertErrorTypesBase { get; }

        public FileDataServer(string filePathServer, string filePathClient,
                              ColorPrint colorPrint, IEnumerable<FileConvertErrorType> fileConvertErrorType)
        {
            string fileType = FileSystemOperations.ExtensionWithoutPointFromPath(filePathServer);
            //string fileName = Path.GetFileNameWithoutExtension(filePathServer);

            if (!ValidFileExtentions.DocAndDgnFileTypes.Keys.Contains(fileType))
            {
                throw new KeyNotFoundException(nameof(filePathServer));
            }
            FileExtentionType = ValidFileExtentions.DocAndDgnFileTypes[fileType.ToLower(CultureInfo.CurrentCulture)];
            FilePathServer = filePathServer;
            FilePathClient = filePathClient;
            ColorPrint = colorPrint;
            // StatusProcessing = StatusProcessing.InQueue;

            FileDatasSourceServerBase = new List<IFileDataSourceServer>();
            FileConvertErrorTypesBase = new List<FileConvertErrorType>(fileConvertErrorType);
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
        public IEnumerable<IFileDataSourceServer> FileDatasSourceServer => FileDatasSourceServer;

        /// <summary>
        /// Тип ошибки при конвертации файла
        /// </summary>
        public IEnumerable<FileConvertErrorType> FileConvertErrorTypes => FileConvertErrorTypesBase;

        /// <summary>
        /// Очистить пути к отконвертированному файлу
        /// </summary>
        protected void ClearConvertedFilePathBase()
        {
            FileDatasSourceServerBase.Clear();
        }

        /// <summary>
        /// Добавить путь к отконвертированному файлу
        /// </summary>
        protected void AddConvertedFilePathBase(IFileDataSourceServer fileDataSourceServer)
        {
            if (fileDataSourceServer != null)
            {
                FileDatasSourceServerBase.Add(fileDataSourceServer);
            }
            else
            {
                throw new ArgumentNullException(nameof(fileDataSourceServer));
            }
        }

        /// <summary>
        /// Добавить пути к отконвертированным файлам
        /// </summary>
        protected void AddRangeConvertedFilePathBase(IEnumerable<IFileDataSourceServer> fileDatasSourceServer)
        {
            if (fileDatasSourceServer != null)
            {
                FileDatasSourceServerBase.AddRange(fileDatasSourceServer);
            }
            else
            {
                throw new ArgumentNullException(nameof(fileDatasSourceServer));
            }
        }

        /// <summary>
        /// Добавить ошибку
        /// </summary>
        public void AddFileConvertErrorType(FileConvertErrorType fileConvertErrorType)
        {
            FileConvertErrorTypesBase.Add(fileConvertErrorType);
        }

        /// <summary>
        /// Добавить ошибки
        /// </summary>
        public void AddRangeFileConvertErrorType(IEnumerable<FileConvertErrorType> fileConvertErrorTypes)
        {
            FileConvertErrorTypesBase.AddRange(fileConvertErrorTypes);
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
