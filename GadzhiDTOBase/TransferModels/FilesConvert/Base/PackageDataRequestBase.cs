using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;

namespace GadzhiDTOBase.TransferModels.FilesConvert.Base
{
    /// <summary>
    /// Базовый класс содержащий данные о конвертируемых файлах
    /// </summary>
    [DataContract]
    public abstract class PackageDataRequestBase<TFileDataRequest>
        where TFileDataRequest: FileDataRequestBase
    {
        protected PackageDataRequestBase(Guid id, ConvertingSettingsRequest convertingSettings, 
                                         IList<TFileDataRequest> filesData)
        {
            Id = id;
            ConvertingSettings = convertingSettings;
            FilesData = filesData;
        }

        /// <summary>
        /// ID идентификатор
        /// </summary>
        [DataMember]
        public Guid Id { get; set; }

        /// <summary>
        /// Параметры конвертации
        /// </summary>
        [DataMember]
        public ConvertingSettingsRequest ConvertingSettings { get; set; }

        /// <summary>
        /// Данные о конвертируемых файлах
        /// </summary>
        [DataMember]
        public IList<TFileDataRequest> FilesData { get; set; }
    }
}
