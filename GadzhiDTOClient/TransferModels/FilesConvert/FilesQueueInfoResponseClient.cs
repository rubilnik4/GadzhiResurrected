using System.Runtime.Serialization;

namespace GadzhiDTOClient.TransferModels.FilesConvert
{
    /// <summary>
    /// Информация о количестве файлов в очереди до текущего пакета
    /// </summary>
    [DataContract]
    public class FilesQueueInfoResponseClient
    {
        public FilesQueueInfoResponseClient(int filesInQueueCount, int packagesInQueueCount)
        {
            FilesInQueueCount = filesInQueueCount;
            PackagesInQueueCount = packagesInQueueCount;
        }

        /// <summary>
        /// Количество файлов в очереди
        /// </summary>
        [DataMember]
        public int FilesInQueueCount { get; private set; }

        /// <summary>
        /// Количество пакетов в очереди
        /// </summary>
        [DataMember]
        public int PackagesInQueueCount { get; private set; }
    }
}
