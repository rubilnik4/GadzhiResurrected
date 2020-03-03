using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GadzhiCommon.Models.Implementations.TransferModels.FilesConvert.Base
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
