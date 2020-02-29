using GadzhiCommon.Models.TransferModels.FilesConvert.Base;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace GadzhiDTOServer.TransferModels.FilesConvert
{
    /// <summary>
    /// Класс содержащий промежуточные данные о конвертируемых файлах для сервера
    /// <summary>
    public class FilesDataIntermediateResponseServer : FilesDataIntermediateResponseBase
    {
        /// <summary>
        /// Промежуточные данные о конвертируемых файлах
        /// </summary>
        [DataMember]
        public IList<FileDataIntermediateResponseServer> FileDatas { get; set; }
    }
}
