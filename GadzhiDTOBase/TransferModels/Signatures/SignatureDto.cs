using System.Collections.Generic;
using System.Runtime.Serialization;
using GadzhiCommon.Models.Interfaces.LibraryData;

namespace GadzhiDTOBase.TransferModels.Signatures
{
    /// <summary>
    /// Идентификатор личности с подписью. Трансферная модель
    /// </summary>
    [DataContract]
    public class SignatureDto: ISignatureFileDataBase<PersonInformationDto>
    {
        public SignatureDto(string personId, PersonInformationDto personInformation, byte[] signatureSource)
        {
            PersonId = personId;
            PersonInformation = personInformation;
            SignatureSourceList = signatureSource;
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
        public byte[] SignatureSourceList { get; private set; }

        /// <summary>
        /// Подпись в формате Jpeg
        /// </summary>
        [IgnoreDataMember]
        public IList<byte> SignatureSource =>
            SignatureSourceList;
    }
}