using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using GadzhiDTOBase.TransferModels.FilesConvert.Base;

namespace GadzhiDTOClient.TransferModels.FilesConvert
{
    /// <summary>
    /// Класс содержащий данные о конвертируемых файлах для клиента
    /// </summary>
    [DataContract]
    public class PackageDataRequestClient : PackageDataRequestBase<FileDataRequestClient>
    {
        public PackageDataRequestClient(Guid id, ConvertingSettingsRequest convertingSettings,
                                        IList<FileDataRequestClient> filesData)
            : base(id, convertingSettings, filesData)
        { }
    }
}
