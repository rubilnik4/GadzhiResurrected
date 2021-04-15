using System;
using System.Collections.Generic;
using System.IO;
using GadzhiApplicationCommon.Extensions.Functional;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Infrastructure.Implementations;
using GadzhiConverting.Models.Interfaces.FilesConvert;

namespace GadzhiConverting.Models.Implementations.FilesConvert
{
    /// <summary>
    /// Путь к файлу, его наименование на серверной и клиентской стороне
    /// </summary>
    public class FilePath : IFilePath
    {
        public FilePath(string filePathServer, string filePathClient, bool onlyDgnAndDocTypes = false)
        {
            if (String.IsNullOrWhiteSpace(filePathServer)) throw new ArgumentNullException(nameof(filePathServer));
            if (String.IsNullOrWhiteSpace(filePathClient)) throw new ArgumentNullException(nameof(filePathClient));

            if (!ValidateExtension(filePathServer, onlyDgnAndDocTypes)) throw new KeyNotFoundException(nameof(filePathServer));
            if (!ValidateExtension(filePathClient, onlyDgnAndDocTypes)) throw new KeyNotFoundException(nameof(filePathClient));

            string fileTypeServer = FileSystemOperations.ExtensionWithoutPointFromPath(filePathServer);
            string fileTypeClient = FileSystemOperations.ExtensionWithoutPointFromPath(filePathClient);
            if (fileTypeServer != fileTypeClient) throw new InvalidOperationException("Расширения клиентской и серверной частей не равны");

            FileExtension = ValidFileExtensions.GetFileTypesValid(fileTypeServer);
            FilePathServer = FileSystemOperations.GetValidFilePath(filePathServer);
            FilePathClient = FileSystemOperations.GetValidFilePath(filePathClient);
        }

        /// <summary>
        /// Путь файла на сервере
        /// </summary>
        public string FilePathServer { get; }

        /// <summary>
        /// Путь файла на сервере
        /// </summary>
        public string FilePathClient { get; }

        /// <summary>
        /// Тип расширения файла
        /// </summary>
        public FileExtension FileExtension { get; }

        /// <summary>
        /// Имя файла на сервере
        /// </summary>
        public string FileNameServer => Path.GetFileName(FilePathServer);

        /// <summary>
        /// Имя файла на клиенте
        /// </summary>
        public string FileNameClient => Path.GetFileName(FilePathClient);

        /// <summary>
        /// Имя файла без расширения на сервере
        /// </summary>
        public string FileNameWithoutExtensionServer => Path.GetFileNameWithoutExtension(FilePathServer);

        /// <summary>
        /// Имя файла без расширения на клиенте
        /// </summary>
        public string FileNameWithoutExtensionClient => Path.GetFileNameWithoutExtension(FileNameClient);

        /// <summary>
        /// Заменить путь к файлу на сервере, проверить расширение
        /// </summary>
        public IFilePath ChangeServerPath(string filePathServer) =>
            Path.GetExtension(filePathServer).
            Map(extension => Path.ChangeExtension(FilePathClient, extension)).
            Map(filePathClient => new FilePath(filePathServer, filePathClient));

        /// <summary>
        /// Изменить имя сервера
        /// </summary>
        public IFilePath ChangeServerName(string serverName) =>
            (!String.IsNullOrWhiteSpace(serverName))
                ? new FilePath(FileSystemOperations.ChangeFilePathNameWithoutExtension(FilePathServer, serverName),
                               FilePathClient)
                : this;

        /// <summary>
        /// Изменить имя клиента
        /// </summary>
        public IFilePath ChangeClientName(string clientName) =>
            (!String.IsNullOrWhiteSpace(clientName))
                ? new FilePath(FilePathServer, FileSystemOperations.ChangeFilePathNameWithoutExtension(FilePathClient, clientName))
                : this;

        /// <summary>
        /// Проверить расширение на соответствие допустимым типам
        /// </summary>
        private static bool ValidateExtension(string filePath, bool onlyDgnAndDocTypes) =>
            FileSystemOperations.ExtensionWithoutPointFromPath(filePath).
            Map(extension => onlyDgnAndDocTypes
                             ? ValidFileExtensions.ContainsInDocAndDgnFileTypes(extension)
                             : ValidFileExtensions.ContainsInFileTypesValid(extension));
    }
}