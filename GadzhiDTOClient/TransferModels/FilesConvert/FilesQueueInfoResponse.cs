using System.Runtime.Serialization;

namespace GadzhiDTOClient.TransferModels.FilesConvert
{
    /// <summary>
    /// Информация о количестве файлов в очереди до текущего пакета
    /// </summary>
    public class FilesQueueInfoResponseClient
    {
        /// <summary>
        /// Количество файлов в очереди
        /// </summary>
        [DataMember]
        public int FilesInQueueCount { get; set; }

        /// <summary>
        /// Количество пакетов в очереди
        /// </summary>
        [DataMember]
        public int PackagesInQueueCount { get; set; }
    }
}
