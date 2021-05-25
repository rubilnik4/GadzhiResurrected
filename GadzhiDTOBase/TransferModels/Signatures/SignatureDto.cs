using System.Runtime.Serialization;

namespace GadzhiDTOBase.TransferModels.Signatures
{
    /// <summary>
    /// Идентификатор личности с подписью. Трансферная модель
    /// </summary>
    [DataContract]
    public class SignatureDto
    {
        public SignatureDto(string personId, PersonInformationDto personInformation, byte[] signatureJpeg)
        {
            PersonId = personId;
            PersonInformation = personInformation;
            SignatureJpeg = signatureJpeg;
        }

        /// <summary>
        /// Идентификатор
        /// </summary>
        [DataMember]
        public string PersonId { get; private set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        [DataMember]
        public PersonInformationDto PersonInformation { get; private set; }

        /// <summary>
        /// Подпись в формате Jpeg
        /// </summary>
        [DataMember]
        public byte[] SignatureJpeg { get; private set; }
    }
}