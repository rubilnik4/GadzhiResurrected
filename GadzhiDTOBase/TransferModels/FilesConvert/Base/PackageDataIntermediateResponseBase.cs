using System;
using System.Runtime.Serialization;
using GadzhiCommon.Enums.FilesConvert;

namespace GadzhiDTOBase.TransferModels.FilesConvert.Base
{
    /// <summary>
    /// Класс содержащий промежуточные данные о конвертируемых файлах
    /// </summary>
    public abstract class PackageDataIntermediateResponseBase
    {
        /// <summary>
        /// ID идентификатор
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
