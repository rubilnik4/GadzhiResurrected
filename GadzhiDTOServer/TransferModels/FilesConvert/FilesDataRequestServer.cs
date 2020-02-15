using GadzhiCommon.Models.TransferModels.FilesConvert.Base;
using GadzhiDTOServer.TransferModels.FilesConvert;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiDTOServer.TransferModels.FilesConvert
{
    /// <summary>
    /// Класс содержащий данные о конвертируемых файлах для сервера
    /// </summary>
    [DataContract]
    public class FilesDataRequestServer: FilesDataRequestBase
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
        public IEnumerable<FileDataRequestServer> FilesData { get; set; }
    }
}
