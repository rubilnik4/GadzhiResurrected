using GadzhiCommon.Models.Implementations.TransferModels.FilesConvert.Base;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace GadzhiDTOClient.TransferModels.FilesConvert
{
    /// <summary>
    /// Класс содержащий данные о отконвертированных файлах для клиента
    /// </summary>
    [DataContract]
    public class PackageDataResponseClient : FilesDataResponseBase
    {
        /// <summary>
        /// Данные о отконвертированных файлах
        /// </summary>
        [DataMember]
        public IList<FileDataResponseClient> FilesData { get; set; }
        
    }
}
