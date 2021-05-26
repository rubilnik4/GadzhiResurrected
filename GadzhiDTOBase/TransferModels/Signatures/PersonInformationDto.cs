using System.Runtime.Serialization;
using GadzhiCommon.Enums.LibraryData;
using GadzhiCommon.Models.Interfaces.LibraryData;

namespace GadzhiDTOBase.TransferModels.Signatures
{
    /// <summary>
    /// Информация о пользователе. Трансферная модель
    /// </summary>
    [DataContract]
    public class PersonInformationDto: IPersonInformation
    {
        public PersonInformationDto(string surname, string name, string patronymic, DepartmentType departmentType)
        {
            Surname = surname;
            Name = name;
            Patronymic = patronymic;
            DepartmentType = departmentType;
        }

        /// <summary>
        /// Фамилия
        /// </summary>
        [DataMember]
        public string Surname { get; private set; }

        /// <summary>
        /// Имя
        /// </summary>
        [DataMember]
        public string Name { get; private set; }

        /// <summary>
        /// Отчество
        /// </summary>
        [DataMember]
        public string Patronymic { get; private set; }

        /// <summary>
        /// Отдел
        /// </summary>
        [DataMember]
        public DepartmentType DepartmentType { get; private set; }
    }
}