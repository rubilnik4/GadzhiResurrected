using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using GadzhiCommon.Enums.FilesConvert;

namespace GadzhiDTOBase.TransferModels.FilesConvert.Base
{
    /// <summary>
    /// Класс содержащий данные о отконвертированных файлах
    /// </summary>
    [DataContract]
    public abstract class PackageDataResponseBase<TFileDataResponse, TFileDataSourceResponse> 
        where TFileDataSourceResponse: FileDataSourceResponseBase
        where TFileDataResponse: FileDataResponseBase<TFileDataSourceResponse>
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
        /// Данные о отконвертированных файлах
        /// </summary>
        [DataMember]
        public abstract IList<TFileDataResponse> FilesData { get; set; }
    }
}
