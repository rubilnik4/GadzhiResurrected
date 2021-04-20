using System.Runtime.Serialization;
using GadzhiCommon.Enums.ConvertingSettings;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Models.Interfaces.LibraryData;
using GadzhiDTOBase.TransferModels.Signatures;

namespace GadzhiDTOBase.TransferModels.FilesConvert.Base
{
    /// <summary>
    /// Параметры конвертации. Трансферная модель
    /// </summary>
    [DataContract]
    public class ConvertingSettingsRequest
    {
        /// <summary>
        /// Идентификатор личной подписи
        /// </summary>
        [DataMember]
        public string PersonId { get; set; }

        /// <summary>
        /// Принцип именования PDF
        /// </summary>
        [DataMember]
        public PdfNamingType PdfNamingType { get; set; }

        /// <summary>
        /// Тип конвертации
        /// </summary>
        [DataMember]
        public ConvertingModeType ConvertingModeType { get; set; }
    }
}
