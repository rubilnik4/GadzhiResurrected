using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using GadzhiCommon.Enums.FilesConvert;

namespace GadzhiDTOBase.TransferModels.FilesConvert.Base
{
    /// <summary>
    /// Класс содержащий промежуточные данные о конвертируемых файлах
    /// </summary>
    [DataContract]
    public abstract class PackageDataShortResponseBase<TFileDataResponse> where TFileDataResponse: FileDataShortResponseBase
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

        /// <summary>
        /// Промежуточные данные о конвертируемых файлах
        /// </summary>
        [DataMember]
        public abstract IList<TFileDataResponse> FilesData { get; set; }
    }
}
