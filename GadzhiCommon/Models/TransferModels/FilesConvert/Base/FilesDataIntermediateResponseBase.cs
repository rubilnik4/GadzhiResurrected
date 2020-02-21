using GadzhiCommon.Enums.FilesConvert;
using System;
using System.Runtime.Serialization;

namespace GadzhiCommon.Models.TransferModels.FilesConvert.Base
{
    /// <summary>
    /// Класс содержащий промежуточные данные о конвертируемых файлах
    /// <summary>
    public class FilesDataIntermediateResponseBase
    {
        /// <summary>
        /// ID идентефикатор
        /// </summary>
        [DataMember]
        public Guid Id { get; set; }

        /// <summary>
        /// Завершена ли обработка
        /// </summary>
        [DataMember]
        public bool IsCompleted { get; set; }

        /// <summary>
        /// Статус выполнения проекта
        /// </summary>
        [DataMember]
        public StatusProcessingProject StatusProcessingProject { get; set; }
    }
}
