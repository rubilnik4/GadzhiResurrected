using System.Runtime.Serialization;

namespace GadzhiDTOServer.TransferModels.Signatures
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
        public string Id { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        [DataMember]
        public string FullName { get; set; }

        /// <summary>
        /// Подпись в формате Jpeg
        /// </summary>
        [DataMember]
        public byte[] SignatureJpeg { get; set; }
    }
}