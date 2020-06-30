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
        public abstract IList<TFileDataRequest> FilesData { get; set; }
    }
}
