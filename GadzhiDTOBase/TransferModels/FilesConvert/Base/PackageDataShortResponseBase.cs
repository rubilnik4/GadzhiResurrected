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
    public abstract class PackageDataShortResponseBase<TFileDataResponse>
        where TFileDataResponse : FileDataShortResponseBase
    {
        protected PackageDataShortResponseBase(Guid id, StatusProcessingProject statusProcessingProject,
                                               IList<TFileDataResponse> filesData)
        {
            Id = id;
            StatusProcessingProject = statusProcessingProject;
            FilesData = filesData;
        }

        /// <summary>
        /// ID идентификатор
        /// </summary>
        [DataMember]
        public Guid Id { get; private set; }

        /// <summary>
        /// Статус выполнения проекта
        /// </summary>
        [DataMember]
        public StatusProcessingProject StatusProcessingProject { get; private set; }

        /// <summary>
        /// Промежуточные данные о конвертируемых файлах
        /// </summary>
        [DataMember]
        public IList<TFileDataResponse> FilesData { get; private set; }
    }
}
