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
                                         ConvertingModeType convertingModeType, bool useDefaultSignature)
        {
            PersonId = personId;
            PdfNamingType = pdfNamingType;
            ConvertingModeType = convertingModeType;
            UseDefaultSignature = useDefaultSignature;
        }

        /// <summary>
        /// Идентификатор личной подписи
        /// </summary>
        [DataMember]
        public string PersonId { get; private set; }

        /// <summary>
        /// Принцип именования PDF
        /// </summary>
        [DataMember]
        public PdfNamingType PdfNamingType { get; private set; }

        /// <summary>
        /// Тип конвертации
        /// </summary>
        [DataMember]
        public ConvertingModeType ConvertingModeType { get; private set; }

        /// <summary>
        /// Использовать подпись по умолчанию
        /// </summary>
        [DataMember]
        public bool UseDefaultSignature { get; private set; }
    }
}
