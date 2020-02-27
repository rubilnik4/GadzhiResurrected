using GadzhiCommon.Models.TransferModels.FilesConvert.Base;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace GadzhiDTOClient.TransferModels.FilesConvert
{
    /// <summary>
    /// Класс содержащий данные о конвертируемых файлах для клиента
    /// </summary>
    [DataContract]
    public class FilesDataRequestClient : FilesDataRequestBase
    {
        /// <summary>
        /// Идентефикация пользователя
        /// </summary>
        [DataMember]
        public string IdentityName { get; set; }

        /// <summary>
        /// Данные о конвертируемых файлах
        /// </summary>
        [DataMember]
        public IList<FileDataRequestClient> FilesData { get; set; }

        /// <summary>
        /// Удовлетворяет ли модель условиям для отправки
        /// </summary>
        [IgnoreDataMember]
        public bool IsValid => FilesData?.Any() == true;
    }
}
