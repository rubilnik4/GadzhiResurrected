using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using GadzhiCommon.Enums.ConvertingSettings;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Models.Interfaces.FilesConvert;
using GadzhiCommon.Models.Interfaces.LibraryData;
using GadzhiDTOBase.TransferModels.Signatures;

namespace GadzhiDTOBase.TransferModels.FilesConvert.Base
{
    /// <summary>
    /// Параметры конвертации. Трансферная модель
    /// </summary>
    [DataContract]
    public class ConvertingSettingsRequest: IConvertingPackageSettings
    {
        public ConvertingSettingsRequest(string personId, PdfNamingType pdfNamingType,
                                         IList<ConvertingModeType> convertingModeTypes, bool useDefaultSignature)
        {
            PersonId = personId;
            PdfNamingType = pdfNamingType;
            ConvertingModeTypesList = convertingModeTypes;
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
        public IList<ConvertingModeType> ConvertingModeTypesList { get; private set; }

        /// <summary>
        /// Тип конвертации
        /// </summary>
        [IgnoreDataMember]
        public IReadOnlyCollection<ConvertingModeType> ConvertingModeTypes =>
            ConvertingModeTypesList.ToList();

        /// <summary>
        /// Использовать подпись по умолчанию
        /// </summary>
        [DataMember]
        public bool UseDefaultSignature { get; private set; }
    }
}
