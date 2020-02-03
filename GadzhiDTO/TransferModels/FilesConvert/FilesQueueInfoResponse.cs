using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiDTO.TransferModels.FilesConvert
{
    /// <summary>
    /// Информация о количестве файлов в очереди до текущего пакета
    /// </summary>
    public class FilesQueueInfoResponse
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
