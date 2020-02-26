using GadzhiMicrostation.Models.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GadzhiMicrostation.Models.Implementations
{
    /// <summary>
    /// Класс для хранения информации о конвертируемом файле типа DGN
    /// </summary>
    public class FileDataMicrostation
    {
        /// <summary>
        /// Пути отконвертированных файлов
        /// </summary>
        private IDictionary<string, FileExtentionType> _convertedFilePathes;

        public FileDataMicrostation(string filePathServer,
                                    string filePathClient,
                                    ColorPrintMicrostation colorPrint)
        {
            _convertedFilePathes = new Dictionary<string, FileExtentionType>();

            if (!String.IsNullOrEmpty(filePathServer))
            {
                string fileType = Path.GetExtension(filePathServer).Trim('.');
                string fileName = Path.GetFileNameWithoutExtension(filePathServer);

                FileExtension = fileType;
                FileName = fileName;
                FilePathServer = filePathServer;
                FilePathClient = filePathClient;
                ColorPrint = colorPrint;
            }
            else
            {
                throw new ArgumentNullException(nameof(filePathServer));
            }
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
        public ColorPrintMicrostation ColorPrint { get; }

        /// <summary>
        /// Пути отконвертированных файлов
        /// </summary>
        public IDictionary<string, FileExtentionType> ConvertedFilePathes =>
            _convertedFilePathes.ToDictionary(pair => pair.Key, pair => pair.Value);

        /// <summary>
        /// Добавить путь к отконвертированному файлу
        /// </summary>
        public void AddConvertedFilePath(string convertedFilePath, FileExtentionType convertedFileExtension)
        {
            if (!String.IsNullOrEmpty(convertedFilePath))
            {
                ConvertedFilePathes?.Add(convertedFilePath, convertedFileExtension);
            }
            else
            {
                throw new ArgumentNullException(nameof(convertedFilePath));
            }
        }
    }
}
