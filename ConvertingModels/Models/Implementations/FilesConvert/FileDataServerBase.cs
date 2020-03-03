using ConvertingModels.Models.Interfaces.FilesConvert;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Infrastructure.Implementations;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace ConvertingModels.Models.Implementations.FilesConvert
{
    /// <summary>
    /// Класс для хранения информации о конвертируемом файле в базовом виде
    /// </summary>
    public abstract class FileDataServerBase : IFileDataServer, IEquatable<FileDataServerBase>
    {
        /// <summary>
        /// Пути отконвертированных файлов
        /// </summary>
        protected List<IFileDataSourceServer> FileDatasSourceServerBase { get; }

        /// <summary>
        /// Тип ошибки при конвертации файла
        /// </summary>
        protected List<FileConvertErrorType> FileConvertErrorTypesBase { get; }

        public FileDataServerBase(string filePathServer, string filePathClient,
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
        /// Добавить путь к отконвертированному файлу
        /// </summary>
        public void AddConvertedFilePath(IFileDataSourceServer fileDataSourceServer)
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
        public void AddRangeConvertedFilePath(IEnumerable<IFileDataSourceServer> fileDatasSourceServer)
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
            return obj is FileDataServerBase fileDataServer &&
                   Equals(fileDataServer);
        }

        public bool Equals(FileDataServerBase other)
        {
            return FilePathServer == other?.FilePathServer;
        }

        public override int GetHashCode()
        {
            return -1576186305 + FilePathServer.GetHashCode();
        }
    }
}
