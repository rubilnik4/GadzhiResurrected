using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiDTOBase.TransferModels.FilesConvert.Base;

namespace GadzhiDTOClient.TransferModels.FilesConvert
{
    /// <summary>
    /// Класс содержащий данные о отконвертированных файлах для клиента
    /// </summary>
    [DataContract]
    public class PackageDataResponseClient : PackageDataResponseBase<FileDataResponseClient, FileDataSourceResponseClient>
    {
        public PackageDataResponseClient(Guid id, StatusProcessingProject statusProcessingProject,
                                         IList<FileDataResponseClient> filesData)
            : base(id, statusProcessingProject, filesData)
        { }
    }
}
