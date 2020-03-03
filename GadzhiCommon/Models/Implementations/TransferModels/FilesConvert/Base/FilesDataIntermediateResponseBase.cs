using GadzhiCommon.Enums.FilesConvert;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GadzhiCommon.Models.Implementations.TransferModels.FilesConvert.Base
{
    /// <summary>
    /// Класс содержащий промежуточные данные о конвертируемых файлах
    /// <summary>
    public abstract class FilesDataIntermediateResponseBase
    {
        /// <summary>
        /// ID идентефикатор
        /// </summary>
        [DataMember]
        public Guid Id { get; set; }       

        /// <summary>
        /// Статус выполнения проекта
        /// </summary>
        [DataMember]
        public StatusProcessingProject StatusProcessingProject { get; set; }
    }
}
