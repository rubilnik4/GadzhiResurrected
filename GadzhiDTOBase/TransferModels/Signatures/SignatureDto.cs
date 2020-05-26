using System.Runtime.Serialization;

namespace GadzhiDTOBase.TransferModels.Signatures
{
    /// <summary>
    /// Идентификатор личности с подписью. Трансферная модель
    /// </summary>
    [DataContract]
    public class SignatureDto
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        [DataMember]
        public string PersonId { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        [DataMember]
        public PersonInformationDto PersonInformation { get; set; }

        /// <summary>
        /// Подпись в формате Jpeg
        /// </summary>
        [DataMember]
        public byte[] SignatureJpeg { get; set; }
    }
}