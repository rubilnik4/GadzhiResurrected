namespace GadzhiCommon.Models.Interfaces.ServerStates
{
    /// <summary>
    /// Информация о очереди на сервере
    /// </summary>
    public interface IServerDetailQueue
    {
        /// <summary>
        /// Текущий пользователь
        /// </summary>
        string CurrentUser { get; }

        /// <summary>
        /// Текущий пакет
        /// </summary>
        string CurrentPackage { get; }

        /// <summary>
        /// Текущий файл
        /// </summary>
        string CurrentFile { get; }

        /// <summary>
        /// Файлов в очереди
        /// </summary>
        int FilesInQueue { get; }

        /// <summary>
        /// Пакетов обработано
        /// </summary>
        int PackagesComplete { get; }

        /// <summary>
        /// Файлов обработано
        /// </summary>
        int FilesComplete { get; }
    }
}