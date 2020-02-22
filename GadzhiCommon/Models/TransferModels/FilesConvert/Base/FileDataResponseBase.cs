using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GadzhiCommon.Models.TransferModels.FilesConvert.Base
{
    /// <summary>
    /// Класс содержащий данные о конвертируемом файле
    /// </summary>
    [DataContract]
    public class FileDataResponseBase : FileDataIntermediateResponseBase
    {
        /// <summary>
        /// Файл данных в формате zip GZipStream
        /// </summary>
        [DataMember]
        public IList<byte> FileDataSource { get; set; }
    }
}
