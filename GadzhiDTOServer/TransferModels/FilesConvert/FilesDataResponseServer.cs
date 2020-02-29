using GadzhiCommon.Models.TransferModels.FilesConvert.Base;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace GadzhiDTOServer.TransferModels.FilesConvert
{
    /// <summary>
    /// Класс содержащий данные о отконвертированных файлах для сервера
    /// </summary>
    [DataContract]
    public class FilesDataResponseServer : FilesDataResponseBase
    {
        /// <summary>
        /// Данные о отконвертированных файлах
        /// </summary>
        [DataMember]
        public IList<FileDataResponseServer> FileDatas { get; set; }       
    }
}
