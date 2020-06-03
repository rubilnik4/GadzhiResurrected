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
        string FilePathServer { get; }

        /// <summary>
        /// Имя файла на клиенте
        /// </summary>
        string FilePathClient { get; }

        /// <summary>
        /// Тип расширения файла
        /// </summary>
        FileExtension FileExtension { get; }

        /// <summary>
        /// Имя файла на сервере
        /// </summary>
        string FileNameServer { get; }

        /// <summary>
        /// Имя файла на клиенте
        /// </summary>
        string FileNameClient { get; }

        /// <summary>
        /// Имя файла без расширения на сервере
        /// </summary>
        string FileNameWithoutExtensionServer { get; }

        /// <summary>
        /// Имя файла без расширения на клиенте
        /// </summary>
        string FileNameWithoutExtensionClient { get; }

        /// <summary>
        /// Заменить путь к файлу на сервере
        /// </summary>
        IFilePath ChangeServerPath(string filePathServer);

        /// <summary>
        /// Изменить имя сервера
        /// </summary>
        IFilePath ChangeServerName(string serverName);


        /// <summary>
        /// Изменить имя клиента
        /// </summary>
        IFilePath ChangeClientName(string clientName);
           
    }
}