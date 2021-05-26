using GadzhiCommon.Enums.LibraryData;

namespace GadzhiCommon.Models.Interfaces.LibraryData
{
    /// <summary>
    /// Информация о пользователе
    /// </summary>
    public interface IPersonInformation
    {
        /// <summary>
        /// Фамилия
        /// </summary>
        string Surname { get; }

        /// <summary>
        /// Имя
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Отчество
        /// </summary>
        string Patronymic { get; }

        /// <summary>
        /// Отдел
        /// </summary>
        DepartmentType DepartmentType { get; }
    }
}