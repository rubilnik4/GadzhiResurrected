using System;
using System.Runtime.Serialization;

namespace GadzhiCommon.Models.TransferModels.FilesConvert.Base
{
    /// <summary>
    /// Базовый класс содержащий данные о конвертируемых файлах
    /// </summary>
    [DataContract]
    public abstract class FilesDataRequestBase
    {
        /// <summary>
        /// ID идентефикатор
        /// </summary>
        [DataMember]
        public Guid Id { get; set; }
    }
}
