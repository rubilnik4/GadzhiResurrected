using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GadzhiDTOBase.TransferModels.FilesConvert.Base
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
