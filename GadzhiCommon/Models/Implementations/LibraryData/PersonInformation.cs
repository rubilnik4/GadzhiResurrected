using System;
using GadzhiCommon.Enums.LibraryData;
using GadzhiCommon.Extensions.Functional;
using GadzhiCommon.Extensions.StringAdditional;
using GadzhiCommon.Helpers.Strings;
using GadzhiCommon.Infrastructure.Implementations.Converters.LibraryData;
using GadzhiCommon.Models.Interfaces.LibraryData;

namespace GadzhiCommon.Models.Implementations.LibraryData
{
    /// <summary>
    /// Информация о пользователе
    /// </summary>
    public readonly struct PersonInformation : IPersonInformation, IEquatable<PersonInformation>
    {
        public PersonInformation(string surname, string name, string patronymic, DepartmentType departmentType )
        {
            Surname = surname ?? String.Empty;
            Name = name ?? String.Empty;
            Patronymic = patronymic ?? String.Empty;
            DepartmentType = departmentType;
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
        public DepartmentType DepartmentType { get; }

        /// <summary>
        /// Отдел в строковом значении
        /// </summary>
        public string Department =>
            ConverterDepartmentType.DepartmentTypeToString(DepartmentType);

        /// <summary>
        /// Полная информация
        /// </summary>
        public string FullName =>
            $"{Surname} {Name} {Patronymic}".Trim();

        /// <summary>
        /// Полная информация
        /// </summary>
        public string FullInformation =>
            $"{FullName} {ConverterDepartmentType.DepartmentTypeToString(DepartmentType)}".Trim();

        /// <summary>
        /// Загружена ли информация о пользователе полностью
        /// </summary>
        public bool HasFullInformation =>
            !String.IsNullOrWhiteSpace(Surname) && !String.IsNullOrWhiteSpace(Name) &&
            !String.IsNullOrWhiteSpace(Patronymic) && DepartmentType != DepartmentType.Unknown;

        /// <summary>
        /// Проверка фамилии 
        /// </summary>
        public bool SurnameEqual(string surname) =>
            String.Equals(Surname, surname, StringComparison.CurrentCultureIgnoreCase);

        /// <summary>
        /// Проверка фамилии и отдела
        /// </summary>
        public bool SurnameAndDepartmentEqual(string surname, DepartmentType department) =>
            SurnameEqual(surname) && DepartmentType == department;

        /// <summary>
        /// Преобразовать строку в информацию о пользователе
        /// </summary>
        public static PersonInformation GetFromFullName(string fullName) =>
            fullName?.Split().
            Map(splat => new PersonInformation(TextFormatting.RemoveSpacesAndArtefacts(splat[0]) ,
                                               TextFormatting.RemoveSpacesAndArtefacts(splat.GetStringFromArrayOrEmpty(1)),
                                               TextFormatting.RemoveSpacesAndArtefacts(splat.GetStringFromArrayOrEmpty(2)),
                                               TextFormatting.RemoveSpacesAndArtefacts(splat.JoinStringArrayFromIndexToEndOrEmpty(3)).
                                                              Map(ConverterDepartmentType.DepartmentStringToTypeOrUnknown)))
            ?? new PersonInformation();

        #region IEquatable
        public override bool Equals(object obj) => obj != null && Equals((PersonInformation)obj);

        public bool Equals(PersonInformation other) =>
            other.Surname == Surname && other.Name == Name && other.Patronymic == Patronymic && other.DepartmentType == DepartmentType;

        public static bool operator ==(PersonInformation left, PersonInformation right) => left.Equals(right);

        public static bool operator !=(PersonInformation left, PersonInformation right) => !(left == right);

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