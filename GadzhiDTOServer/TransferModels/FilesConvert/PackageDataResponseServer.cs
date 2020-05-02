using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using GadzhiDTOBase.TransferModels.FilesConvert.Base;

namespace GadzhiDTOServer.TransferModels.FilesConvert
{
    /// <summary>
    /// Класс содержащий данные о отконвертированных файлах для сервера
    /// </summary>
    [DataContract]
    public class PackageDataResponseServer : PackageDataResponseBase
    {
        /// <summary>
        /// Данные о отконвертированных файлах
        /// </summary>
        [DataMember]
        public IList<FileDataResponseServer> FilesData { get; set; }       
    }
}
