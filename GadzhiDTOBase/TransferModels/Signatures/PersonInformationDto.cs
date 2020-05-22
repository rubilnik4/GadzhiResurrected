using System.Runtime.Serialization;

namespace GadzhiDTOBase.TransferModels.Signatures
{
    /// <summary>
    /// Информация о пользователе. Трансферная модель
    /// </summary>
    [DataContract]
    public class PersonInformationDto
    {
        /// <summary>
        /// Фамилия
        /// </summary>
        [DataMember]
        public string Surname { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        [DataMember]
        public string Patronymic { get; set; }

        /// <summary>
        /// Отдел
        /// </summary>
        [DataMember]
        public string Department { get; set; }
    }
}