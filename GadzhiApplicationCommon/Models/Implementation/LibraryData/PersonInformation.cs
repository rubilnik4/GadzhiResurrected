using System;
using GadzhiApplicationCommon.Extensions.Functional;
using GadzhiApplicationCommon.Extensions.StringAdditional;

namespace GadzhiApplicationCommon.Models.Implementation.LibraryData
{
    /// <summary>
    /// Информация о пользователе
    /// </summary>
    public readonly struct PersonInformation : IEquatable<PersonInformation>
    {
        public PersonInformation(string surname, string name, string patronymic, string department)
        {
            Surname = !surname.IsNullOrWhiteSpace()
                      ? surname
                      : throw new ArgumentNullException(nameof(surname));
            Name = name ?? String.Empty;
            Patronymic = patronymic ?? String.Empty;
            Department = department ?? String.Empty;
        }

        /// <summary>
        /// Фамилия
        /// </summary>
        public string Surname { get; }

        /// <summary>
        /// Имя
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Отчество
        /// </summary>
        public string Patronymic { get; }

        /// <summary>
        /// Отдел
        /// </summary>
        public string Department { get; }

        /// <summary>
        /// Проверка фамилии 
        /// </summary>
        public bool SurnameEqual(string surname) => String.Equals(Surname, surname, StringComparison.CurrentCultureIgnoreCase);

        /// <summary>
        /// Проверка отдела 
        /// </summary>
        public bool DepartmentEqual(string department) => String.Equals(Department, department, StringComparison.CurrentCultureIgnoreCase);

        /// <summary>
        /// Проверка фамилии и отдела
        /// </summary>
        public bool SurnameAndDepartmentEqual(string surname, string department) => SurnameEqual(surname) && DepartmentEqual(department);

        /// <summary>
        /// Преобразовать строку в информацию о пользователе
        /// </summary>
        public static PersonInformation GetFromFullName(string fullName) =>
            fullName?.Split(null).
            Map(splat => new PersonInformation(splat[0],
                                               splat.GetStringFromArrayOrEmpty(1),
                                               splat.GetStringFromArrayOrEmpty(2),
                                               splat.JoinStringArrayFromIndexToEndOrEmpty(3)))
            ?? new PersonInformation();

        #region IEquatable
        public override bool Equals(object obj) => obj != null && Equals((PersonInformation)obj);

        public bool Equals(PersonInformation other) =>
            other.Surname == Surname && other.Name == Name && other.Patronymic == Patronymic && other.Department == Department;

        public static bool operator ==(PersonInformation left, PersonInformation right) => left.Equals(right);

        public static bool operator !=(PersonInformation left, PersonInformation right) => !(left == right);

        public override int GetHashCode()
        {
            var hashCode = 17;
            hashCode = hashCode * 31 + Surname.GetHashCode();
            hashCode = hashCode * 31 + Name.GetHashCode();
            hashCode = hashCode * 31 + Patronymic.GetHashCode();
            hashCode = hashCode * 31 + Department.GetHashCode();

            return hashCode;
        }
        #endregion
    }
}