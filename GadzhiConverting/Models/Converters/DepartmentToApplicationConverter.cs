using System;
using GadzhiApplicationCommon.Models.Enums.LibraryData;
using GadzhiCommon.Enums.LibraryData;
using GadzhiCommon.Infrastructure.Implementations.Converters.LibraryData;
using GadzhiConverting.Extensions;

namespace GadzhiConverting.Models.Converters
{
    /// <summary>
    /// Конвертировать тип отдела в версию для приложения
    /// </summary>
    public static class DepartmentToApplicationConverter
    {
        /// <summary>
        /// Конвертировать тип цвета печати в версию для приложения
        /// </summary>
        public static DepartmentTypeApp ToApplication(DepartmentType departmentType) =>
            Enum.TryParse(departmentType.ToString(), true, out DepartmentTypeApp departmentTypeApplication) ?
                departmentTypeApplication :
                throw new FormatException(nameof(departmentType));

        /// <summary>
        /// Конвертировать тип цвета печати в версию из приложения
        /// </summary>
        public static DepartmentType FromApplication(DepartmentTypeApp departmentTypeApp) =>
            Enum.TryParse(departmentTypeApp.ToString(), true, out DepartmentType departmentType) ?
                departmentType :
                throw new FormatException(nameof(departmentTypeApp));

        /// <summary>
        /// Получить функцию преобразования типа отдела в строку
        /// </summary>
        public static Func<DepartmentTypeApp, string> GetDepartmentTypeFunc() =>
            departmentTypeApp => ConverterDepartmentType.DepartmentTypeToString(departmentTypeApp.FromApplication());
    }
}