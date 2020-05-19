using GadzhiCommon.Enums.FilesConvert;

namespace GadzhiConverting.Models.Interfaces.FilesConvert
{
    /// <summary>
    /// Путь к файлу, его наименование на серверной и клиентской стороне
    /// </summary>
    public interface IFilePath
    {
        /// <summary>
        /// Путь файла на сервере
        /// </summary>
        public string FilePathServer { get; }

        /// <summary>
        /// Имя файла на клиенте
        /// </summary>
        public string FilePathClient { get; }

        /// <summary>
        /// Тип расширения файла
        /// </summary>
        public FileExtension FileExtension { get; }

        /// <summary>
        /// Имя файла на сервере
        /// </summary>
        public string FileNameServer { get; }

        /// <summary>
        /// Имя файла на клиенте
        /// </summary>
        public string FileNameClient { get; }

        /// <summary>
        /// Имя файла без расширения на сервере
        /// </summary>
        public string FileNameWithoutExtensionServer { get; }

        /// <summary>
        /// Имя файла без расширения на клиенте
        /// </summary>
        public string FileNameWithoutExtensionClient { get; }

        /// <summary>
        /// Заменить путь к файлу на сервере
        /// </summary>
        public IFilePath ChangeServerPath(string filePathServer);
    }
}