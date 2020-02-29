using GadzhiCommon.Models.TransferModels.FilesConvert.Base;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace GadzhiDTOClient.TransferModels.FilesConvert
{
    /// <summary>
    /// Класс содержащий промежуточные данные о конвертируемых файлах  для клиента
    /// <summary>
    public class FilesDataIntermediateResponseClient : FilesDataIntermediateResponseBase
    {
        /// <summary>
        /// Промежуточные данные о конвертируемых файлах
        /// </summary>
        [DataMember]
        public IList<FileDataIntermediateResponseClient> FileDatas { get; set; }

        /// <summary>
        /// Информация о количестве файлов в очереди до текущего пакета
        /// </summary>
        [DataMember]
        public FilesQueueInfoResponseClient FilesQueueInfo { get; set; }
    }
}
