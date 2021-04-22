using System.Runtime.Serialization;
using GadzhiCommon.Models.Interfaces.ServerStates;

namespace GadzhiDTOBase.TransferModels.ServerStates
{
    /// <summary>
    /// Информация о очереди на сервере. Трансферная модель
    /// </summary>
    [DataContract]
    public class ServerDetailQueueResponse: IServerDetailQueue
    {
        public ServerDetailQueueResponse(string currentUser, string currentPackage, string currentFile,
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
        [DataMember]
        public string CurrentUser { get; private set; }

        /// <summary>
        /// Текущий пакет
        /// </summary>
        [DataMember]
        public string CurrentPackage { get; private set; }

        /// <summary>
        /// Текущий файл
        /// </summary>
        [DataMember]
        public string CurrentFile { get; private set; }

        /// <summary>
        /// Файлов в очереди
        /// </summary>
        [DataMember]
        public int FilesInQueue { get; private set; }

        /// <summary>
        /// Пакетов обработано
        /// </summary>
        [DataMember]
        public int PackagesComplete { get; private set; }

        /// <summary>
        /// Файлов обработано
        /// </summary>
        [DataMember]
        public int FilesComplete { get; private set; }
    }
}