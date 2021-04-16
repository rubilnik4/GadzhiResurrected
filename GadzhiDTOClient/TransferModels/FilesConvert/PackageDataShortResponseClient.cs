using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using GadzhiDTOBase.TransferModels.FilesConvert.Base;

namespace GadzhiDTOClient.TransferModels.FilesConvert
{
    /// <summary>
    /// Класс содержащий промежуточные данные о конвертируемых файлах  для клиента
    /// </summary>
    public class PackageDataShortResponseClient : PackageDataShortResponseBase<FileDataShortResponseClient>
    {
        /// <summary>
        /// Промежуточные данные о конвертируемых файлах
        /// </summary>
        [DataMember]
        public override IList<FileDataShortResponseClient> FilesData { get; set; }

        /// <summary>
        /// Информация о количестве файлов в очереди до текущего пакета
        /// </summary>
        [DataMember]
        public FilesQueueInfoResponseClient FilesQueueInfo { get; set; }
    }
}
