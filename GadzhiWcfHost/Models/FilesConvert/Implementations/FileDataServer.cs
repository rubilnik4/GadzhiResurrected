using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiWcfHost.Models.FilesConvert.Implementations
{
    /// <summary>
    /// Класс для хранения информации о конвертируемом файле
    /// </summary>
    public class FileDataServer : IEquatable<FileDataServer>
    {
        public FileDataServer(string filePathServer, string filePathClient, ColorPrint colorPrint)
              : this(filePathServer, filePathClient, colorPrint, new List<FileConvertErrorType>())
        {

        }

        public FileDataServer(string filePathServer, string filePathClient, ColorPrint colorPrint, List<FileConvertErrorType> fileConvertErrorType)
        {
            string fileType = FileHelpers.ExtensionWithoutPointFromPath(filePathServer);
            string fileName = Path.GetFileNameWithoutExtension(filePathServer);
            if (String.IsNullOrEmpty(fileType) || String.IsNullOrEmpty(fileName) || String.IsNullOrEmpty(filePathServer))
            {
                throw new ArgumentNullException("Входные параметры FileData имеют пустое значение");
            }

            FileExtension = fileType;
            FileName = fileName;
            FilePathServer = filePathServer;
            FilePathClient = filePathClient;
            ColorPrint = colorPrint;

            FileConvertErrorType = fileConvertErrorType;
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
        public List<FileConvertErrorType> FileConvertErrorType { get; }

        /// <summary>
        /// Корректна ли модель
        /// </summary>
        public bool IsValid => FileConvertErrorType == null || FileConvertErrorType.Count == 0;

        public bool Equals(FileDataServer other)
        {
            return FilePathServer == other.FilePathServer;
        }
    }
}
