using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using GadzhiDTOBase.TransferModels.FilesConvert.Base;

namespace GadzhiDTOClient.TransferModels.FilesConvert
{
    /// <summary>
    /// Класс содержащий данные о конвертируемых файлах для клиента
    /// </summary>
    [DataContract]
    public class PackageDataRequestClient : PackageDataRequestBase<FileDataRequestClient>
    {
        /// <summary>
        /// Данные о конвертируемых файлах
        /// </summary>
        [DataMember]
        public override IList<FileDataRequestClient> FilesData { get; set; }

        /// <summary>
        /// Удовлетворяет ли модель условиям для отправки
        /// </summary>
        [IgnoreDataMember]
        public bool IsValid => FilesData?.Any() == true;
    }
}
