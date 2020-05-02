using System;
using System.Runtime.Serialization;
using GadzhiCommon.Enums.FilesConvert;

namespace GadzhiDTOBase.TransferModels.FilesConvert.Base
{
    /// <summary>
    /// Класс содержащий данные о отконвертированных файлах
    /// </summary>
    [DataContract]
    public abstract class PackageDataResponseBase
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
