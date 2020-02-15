using GadzhiCommon.Enums.FilesConvert;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiCommon.Models.TransferModels.FilesConvert.Base
{
    /// <summary>
    /// Класс содержащий данные о конвертируемом файле
    /// </summary>
    [DataContract]
    public class FileDataResponseBase: FileDataIntermediateResponseBase      
    {
        /// <summary>
        /// Файл данных в формате zip GZipStream
        /// </summary>
        [DataMember]
        public byte[] FileDataSource { get; set; }      
    }
}
