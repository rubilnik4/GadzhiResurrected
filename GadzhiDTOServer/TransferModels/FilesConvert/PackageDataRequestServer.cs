using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using GadzhiDTOBase.TransferModels.FilesConvert.Base;

namespace GadzhiDTOServer.TransferModels.FilesConvert
{
    /// <summary>
    /// Класс содержащий данные о конвертируемых файлах для сервера
    /// </summary>
    [DataContract]
    public class PackageDataRequestServer : PackageDataRequestBase
    {
        /// <summary>
        /// Количество попыток конвертирования
        /// </summary>   
        [DataMember]
        public int AttemptingConvertCount { get; set; }

        /// <summary>
        /// Данные о конвертируемых файлах
        /// </summary>
        [DataMember]
        public IList<FileDataRequestServer> FilesData { get; set; }       
    }
}
