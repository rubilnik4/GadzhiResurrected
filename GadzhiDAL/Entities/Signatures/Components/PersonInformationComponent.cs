using System;
using GadzhiCommon.Enums.LibraryData;
using GadzhiCommon.Models.Interfaces.LibraryData;

// ReSharper disable VirtualMemberCallInConstructor
namespace GadzhiDAL.Entities.Signatures.Components
{
    /// <summary>
    /// Информация о пользователе. Компонент в базе данных
    /// </summary>
    public class PersonInformationComponent: IPersonInformation
    {
        public PersonInformationComponent()
        { }

        public PersonInformationComponent(string surname, string name, string patronymic, DepartmentType departmentType)
        {
            Surname = surname;
            Name = name;
            Patronymic = patronymic;
            DepartmentType = departmentType;
        }

        /// <summary>
        /// Фамилия
        /// </summary>
        public virtual string Surname { get; protected set; }

        /// <summary>
        /// Имя
        /// </summary>
        public virtual string Name { get; protected set; }

        /// <summary>
        /// Отчество
        /// </summary>
        public virtual string Patronymic { get; protected set; }

        /// <summary>
        /// Отдел
        /// </summary>
        public virtual DepartmentType DepartmentType { get; protected set; }
    }
}