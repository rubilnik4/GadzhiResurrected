using System;
using System.Runtime.Serialization;

namespace GadzhiDTOBase.TransferModels.FilesConvert.Base
{
    /// <summary>
    /// Базовый класс содержащий данные о конвертируемых файлах
    /// </summary>
    [DataContract]
    public abstract class PackageDataRequestBase
    {
        /// <summary>
        /// ID идентификатор
        /// </summary>
        [DataMember]
        public Guid Id { get; set; }
    }
}
