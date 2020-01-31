using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiDTO.TransferModels.FilesConvert
{
    /// <summary>
    /// Класс содержащий данные о отконвертированных файлах
    /// </summary>
    [DataContract]
    public class FilesDataResponse
    {       
        /// <summary>
        /// Данные о отконвертированных файлах
        /// </summary>
        [DataMember]
        public IEnumerable<FileDataResponse> FilesData { get; set; }       
    }
}
