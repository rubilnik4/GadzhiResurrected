using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiDTO.TransferModels.FilesConvert
{
    /// <summary>
    /// Класс содержащий данные о конвертируемых файлах
    /// </summary>
    [DataContract]
    public class FilesDataDTO
    {
        /// <summary>
        /// Данные о конвертируемых файлах
        /// </summary>
        [DataMember]
        public IEnumerable<FileDataDTO> Files { get; set; }
    }
}
