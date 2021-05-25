using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiDTOBase.TransferModels.FilesConvert.Base;

namespace GadzhiDTOClient.TransferModels.FilesConvert
{
    /// <summary>
    /// Класс содержащий промежуточные данные о конвертируемых файлах  для клиента
    /// </summary>
    [DataContract]
    public class PackageDataShortResponseClient : PackageDataShortResponseBase<FileDataShortResponseClient>
    {
        public PackageDataShortResponseClient(Guid id, StatusProcessingProject statusProcessingProject,
                                              IList<FileDataShortResponseClient> filesData,
                                              FilesQueueInfoResponseClient filesQueueInfo)
            : base(id, statusProcessingProject, filesData)
        {
            FilesQueueInfo = filesQueueInfo;
        }

        /// <summary>
        /// Информация о количестве файлов в очереди до текущего пакета
        /// </summary>
        [DataMember]
        public FilesQueueInfoResponseClient FilesQueueInfo { get; private set; }
    }
}
