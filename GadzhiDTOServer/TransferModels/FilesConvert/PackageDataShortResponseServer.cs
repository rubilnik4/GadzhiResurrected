using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using GadzhiDTOBase.TransferModels.FilesConvert.Base;

namespace GadzhiDTOServer.TransferModels.FilesConvert
{
    /// <summary>
    /// Класс содержащий промежуточные данные о конвертируемых файлах для сервера
    /// </summary>
    [DataContract]
    public class PackageDataShortResponseServer : PackageDataShortResponseBase<FileDataShortResponseServer>
    {
        /// <summary>
        /// Промежуточные данные о конвертируемых файлах
        /// </summary>
        [DataMember]
        public override IList<FileDataShortResponseServer> FilesData { get; set; }
    }
}
