using System;
using GadzhiCommon.Enums.LibraryData;

namespace GadzhiDAL.Entities.Signatures.Components
{
    /// <summary>
    /// Информация о пользователе. Компонент в базе данных
    /// </summary>
    public class PersonInformationComponent
    {
        /// <summary>
        /// Фамилия
        /// </summary>
        public virtual string Surname { get; set; } = String.Empty;

        /// <summary>
        /// Имя
        /// </summary>
        public virtual string Name { get; set; } = String.Empty;

        /// <summary>
        /// Отчество
        /// </summary>
        public virtual string Patronymic { get; set; } = String.Empty;

        /// <summary>
        /// Отдел
        /// </summary>
        public virtual DepartmentType DepartmentType { get; set; } = DepartmentType.Unknown;
    }
}