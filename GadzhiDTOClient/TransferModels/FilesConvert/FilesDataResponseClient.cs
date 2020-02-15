using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Models.TransferModels.FilesConvert.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiDTOClient.TransferModels.FilesConvert
{
    /// <summary>
    /// Класс содержащий данные о отконвертированных файлах для клиента
    /// </summary>
    [DataContract]
    public class FilesDataResponseClient: FilesDataResponseBase
    {
        /// <summary>
        /// Данные о отконвертированных файлах
        /// </summary>
        [DataMember]
        public IEnumerable<FileDataResponseClient> FilesData { get; set; }       
    }
}
