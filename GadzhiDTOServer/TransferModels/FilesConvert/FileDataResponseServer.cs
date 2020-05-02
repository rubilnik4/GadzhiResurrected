using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using GadzhiDTOBase.TransferModels.FilesConvert.Base;

namespace GadzhiDTOServer.TransferModels.FilesConvert
{
    /// <summary>
    /// Класс содержащий данные о конвертируемом файле для сервера
    /// </summary>
    [DataContract]
    public class FileDataResponseServer : FileDataResponseBase
    {
        /// <summary>
        /// Информация об отконвертированных файлах в серверной части
        /// </summary>
        [DataMember]
        public IList<FileDataSourceResponseServer> FilesDataSourceResponseServer { get; set; }

    }
}
