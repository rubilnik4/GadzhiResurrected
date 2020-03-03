using GadzhiCommon.Models.Implementations.TransferModels.FilesConvert.Base;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace GadzhiDTOServer.TransferModels.FilesConvert
{
    /// <summary>
    /// Класс содержащий данные о конвертируемых файлах для сервера
    /// </summary>
    [DataContract]
    public class FilesDataRequestServer : FilesDataRequestBase
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
        public IList<FileDataRequestServer> FileDatas { get; set; }       
    }
}
