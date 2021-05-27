using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiDTOBase.TransferModels.FilesConvert.Base;

namespace GadzhiDTOServer.TransferModels.FilesConvert
{
    /// <summary>
    /// Класс содержащий промежуточные данные о конвертируемых файлах для сервера
    /// </summary>
    [DataContract]
    public class PackageDataShortResponseServer : PackageDataShortResponseBase<FileDataShortResponseServer>
    {
        public PackageDataShortResponseServer(Guid id, StatusProcessingProject statusProcessingProject,
                                              IList<FileDataShortResponseServer> filesData)
            :base(id, statusProcessingProject, filesData)
        { }
    }
}
