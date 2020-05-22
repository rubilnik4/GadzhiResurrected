using System;

namespace GadzhiDAL.Entities.Signatures
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
        public virtual string Department { get; set; } = String.Empty;
    }
}