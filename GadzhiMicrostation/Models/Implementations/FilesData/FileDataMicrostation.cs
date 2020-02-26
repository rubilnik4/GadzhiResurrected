using GadzhiMicrostation.Models.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GadzhiMicrostation.Infrastructure.Implementations;

namespace GadzhiMicrostation.Models.Implementations.FilesData
{
    /// <summary>
    /// Класс для хранения информации о конвертируемом файле типа DGN
    /// </summary>
    public class FileDataMicrostation
    {
        /// <summary>
        /// Пути отконвертированных файлов
        /// </summary>
        private readonly IList<ConvertedFileDataMicrostation> _convertedFileDataMicrostation;

        /// <summary>
        /// Тип ошибки при конвертации файла
        /// </summary>
        private readonly IList<ErrorMicrostationType> _fileConvertErrorTypes;

        public FileDataMicrostation(string filePathServer,
                                    string filePathClient,
                                    ColorPrintMicrostation colorPrint)
        {
            _convertedFileDataMicrostation = new List<ConvertedFileDataMicrostation>();
            _fileConvertErrorTypes = new List<ErrorMicrostationType>();

            if (!String.IsNullOrEmpty(filePathServer))
            {
                string fileExtention = FileSystemOperationsMicrostation.ExtensionWithoutPointFromPath(filePathServer);
                string fileName = Path.GetFileNameWithoutExtension(filePathServer);

                var fileExtensionType = Enum.Parse(typeof(FileExtentionMicrostation), fileExtention, true);
                if (fileExtensionType is FileExtentionMicrostation)
                {
                    FileExtension = (FileExtentionMicrostation)fileExtensionType;
                }
                else
                {
                    throw new FormatException(nameof(fileExtention));
                }

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
        public FileExtentionMicrostation FileExtension { get; }

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
        public IEnumerable<ConvertedFileDataMicrostation> ConvertedFileDataMicrostation => _convertedFileDataMicrostation;

        /// <summary>
        /// Тип ошибки при конвертации файла
        /// </summary>
        public IEnumerable<ErrorMicrostationType> FileConvertErrorTypes => _fileConvertErrorTypes;

        /// <summary>
        /// Добавить путь к отконвертированному файлу
        /// </summary>
        public void AddConvertedFilePath(ConvertedFileDataMicrostation convertedFileDataMicrostation)
        {
            if (convertedFileDataMicrostation != null)
            {
                _convertedFileDataMicrostation.Add(convertedFileDataMicrostation);
            }
            else
            {
                throw new ArgumentNullException(nameof(convertedFileDataMicrostation));
            }
        }

        /// <summary>
        /// Добавить ошибку конвертации
        /// </summary>
        public void AddFileConvertErrorType(ErrorMicrostationType errorMicrostationType)
        {
            _fileConvertErrorTypes.Add(errorMicrostationType);
        }
    }
}
