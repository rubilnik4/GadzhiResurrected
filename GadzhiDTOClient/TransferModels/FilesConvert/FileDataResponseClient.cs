using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using GadzhiDTOBase.TransferModels.FilesConvert.Base;

namespace GadzhiDTOClient.TransferModels.FilesConvert
{
    /// <summary>
    /// Класс содержащий данные о конвертируемом файле для клиента
    /// </summary>
    [DataContract]
    public class FileDataResponseClient : FileDataResponseBase
    {        
        /// <summary>
        /// Информация об отконвертированных файлах в клиентской части
        /// </summary>
        [DataMember]
        public IList<FileDataSourceResponseClient> FilesDataSourceResponseClient { get; set; }        
    }
}
