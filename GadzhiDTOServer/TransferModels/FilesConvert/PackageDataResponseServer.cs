using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiDTOBase.TransferModels.FilesConvert.Base;

namespace GadzhiDTOServer.TransferModels.FilesConvert
{
    /// <summary>
    /// Класс содержащий данные о отконвертированных файлах для сервера
    /// </summary>
    [DataContract]
    public class PackageDataResponseServer : PackageDataResponseBase<FileDataResponseServer, FileDataSourceResponseServer>
    {
        public PackageDataResponseServer(Guid id, StatusProcessingProject statusProcessingProject,
                                         IList<FileDataResponseServer> filesData)
            :base(id, statusProcessingProject, filesData)
        { }
    }
}
