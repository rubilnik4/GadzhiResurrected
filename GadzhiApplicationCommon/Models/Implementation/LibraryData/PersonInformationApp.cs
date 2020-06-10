using GadzhiApplicationCommon.Extensions.Functional;
using GadzhiApplicationCommon.Extensions.StringAdditional;
using System;
using GadzhiApplicationCommon.Models.Enums.LibraryData;

namespace GadzhiApplicationCommon.Models.Implementation.LibraryData
{
    /// <summary>
    /// Информация о пользователе
    /// </summary>
    public readonly struct PersonInformationApp : IEquatable<PersonInformationApp>
    {
        private readonly Func<DepartmentTypeApp, string> _departmentToString;

        public PersonInformationApp(string surname, string name, string patronymic, DepartmentTypeApp departmentType, 
                                    Func<DepartmentTypeApp, string> departmentToString)
        {
            Surname = !surname.IsNullOrWhiteSpace()
                      ? surname
                      : throw new ArgumentNullException(nameof(surname));
            Name = name ?? String.Empty;
            Patronymic = patronymic ?? String.Empty;
            DepartmentType = departmentType;
            _departmentToString = departmentToString ?? throw new ArgumentNullException(nameof(departmentToString));
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
        public DepartmentTypeApp DepartmentType { get; }

        /// <summary>
        /// Отдел в строковом значении
        /// </summary>
        private string Department => _departmentToString(DepartmentType);

        /// <summary>
        /// Полная информация
        /// </summary>
        public string FullName => $"{Surname} {Name} {Patronymic}".Trim();

        /// <summary>
        /// Полная информация
        /// </summary>
        public string FullInformation => $"{FullName} {Department}".Trim();

        /// <summary>
        /// Загружена ли информация о пользователе полностью
        /// </summary>
        public bool HasFullInformation => !Surname.IsNullOrWhiteSpace() && !Name.IsNullOrWhiteSpace() &&
                                          !Patronymic.IsNullOrWhiteSpace() && DepartmentType != DepartmentTypeApp.Unknown;

        /// <summary>
        /// Проверка фамилии 
        /// </summary>
        public bool SurnameEqual(string surname) => String.Equals(Surname, surname, StringComparison.CurrentCultureIgnoreCase);


        /// <summary>
        /// Проверка фамилии и отдела
        /// </summary>
        public bool SurnameAndDepartmentEqual(string surname, DepartmentTypeApp departmentType) => 
            SurnameEqual(surname) && DepartmentType == departmentType;

        #region IEquatable
        public override bool Equals(object obj) => obj != null && Equals((PersonInformationApp)obj);

        public bool Equals(PersonInformationApp other) =>
            other.Surname == Surname && other.Name == Name && other.Patronymic == Patronymic && other.DepartmentType == DepartmentType;

        public static bool operator ==(PersonInformationApp left, PersonInformationApp right) => left.Equals(right);

        public static bool operator !=(PersonInformationApp left, PersonInformationApp right) => !(left == right);

        public override int GetHashCode()
        {
            var hashCode = 17;
            hashCode = hashCode * 31 + Surname.GetHashCode();
            hashCode = hashCode * 31 + Name.GetHashCode();
            hashCode = hashCode * 31 + Patronymic.GetHashCode();
            hashCode = hashCode * 31 + DepartmentType.GetHashCode();

            return hashCode;
        }
        #endregion
    }
}