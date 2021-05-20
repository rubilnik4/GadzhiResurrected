using System.Collections.Generic;
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
        public ConvertingSettingsRequest(string personId, PdfNamingType pdfNamingType,
                                         IList<ConvertingModeType> convertingModeTypes, bool useDefaultSignature)
        {
            PersonId = personId;
            PdfNamingType = pdfNamingType;
            ConvertingModeTypes = convertingModeTypes;
            UseDefaultSignature = useDefaultSignature;
        }

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
        public IList<ConvertingModeType> ConvertingModeTypes { get; set; }

        /// <summary>
        /// Использовать подпись по умолчанию
        /// </summary>
        [DataMember]
        public bool UseDefaultSignature { get; set; }
    }
}
