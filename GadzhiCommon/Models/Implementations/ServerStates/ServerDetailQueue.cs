using GadzhiCommon.Models.Interfaces.ServerStates;

namespace GadzhiCommon.Models.Implementations.ServerStates
{
    /// <summary>
    /// Информация о очереди на сервере
    /// </summary>
    public class ServerDetailQueue: IServerDetailQueue
    {
        public ServerDetailQueue(string currentUser, string currentPackage, string currentFile,
                                 int filesInQueue, int packagesComplete, int filesComplete)
        {
            CurrentUser = currentUser;
            CurrentPackage = currentPackage;
            CurrentFile = currentFile;
            FilesInQueue = filesInQueue;
            PackagesComplete = packagesComplete;
            FilesComplete = filesComplete;
        }

        /// <summary>
        /// Текущий пользователь
        /// </summary>
        public string CurrentUser { get; }

        /// <summary>
        /// Текущий пакет
        /// </summary>
        public string CurrentPackage { get; }

        /// <summary>
        /// Текущий файл
        /// </summary>
        public string CurrentFile { get; }

        /// <summary>
        /// Файлов в очереди
        /// </summary>
        public int FilesInQueue { get; }

        /// <summary>
        /// Пакетов обработано
        /// </summary>
        public int PackagesComplete { get; }

        /// <summary>
        /// Файлов обработано
        /// </summary>
        public int FilesComplete { get; }
    }
}