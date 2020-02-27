using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiCommon.Models.TransferModels.FilesConvert.Base
{
    /// <summary>
    /// Информация об отконвертированных файлах
    /// </summary>
    [DataContract]
    public abstract class FileDataSourceResponseBase
    {
        /// <summary>
        /// Путь файла
        /// </summary>
        [DataMember]
        public string FileName { get; set; }
         
        /// <summary>
        /// Файл данных в формате zip GZipStream
        /// </summary>
        [DataMember]
        public IList<byte> FileDataSource { get; set; }
    }
}
