using GadzhiApplicationCommon.Models.Enums.LibraryData;
using GadzhiCommon.Enums.LibraryData;
using GadzhiConverting.Models.Converters;
using System;

namespace GadzhiConverting.Extensions
{
    /// <summary>
    /// Методы расширения для типа отдела
    /// </summary>
    public static class DepartmentTypeExtensions
    {
        /// <summary>
        /// Конвертировать тип отдела в версию для приложения
        /// </summary>
        public static DepartmentTypeApp ToApplication(this DepartmentType departmentType) =>
            DepartmentToApplicationConverter.ToApplication(departmentType);

        /// <summary>
        /// Конвертировать тип отдела в версию из приложения
        /// </summary>
        public static DepartmentType FromApplication(this DepartmentTypeApp departmentTypeApp) =>
            DepartmentToApplicationConverter.FromApplication(departmentTypeApp);
    }
}