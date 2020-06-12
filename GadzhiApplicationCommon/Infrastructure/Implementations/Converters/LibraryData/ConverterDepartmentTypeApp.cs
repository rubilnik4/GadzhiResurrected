using GadzhiApplicationCommon.Extensions.StringAdditional;
using GadzhiApplicationCommon.Models.Enums.LibraryData;

namespace GadzhiApplicationCommon.Infrastructure.Implementations.Converters.LibraryData
{
    /// <summary>
    /// Операции над типом отдела
    /// </summary>
    public static class ConverterDepartmentTypeApp
    {
        /// <summary>
        /// Поиск типа отдела по маркерам
        /// </summary>
        public static DepartmentTypeApp DepartmentParsing(string departmentText) =>
            departmentText switch
            {
                _ when departmentText.Contains("АСУ ТП") => DepartmentTypeApp.AsuTp,
                _ when departmentText.ContainsIgnoreCase("систем связ") => DepartmentTypeApp.AsuTp,
                _ when departmentText.ContainsIgnoreCase("архитектурн") => DepartmentTypeApp.Aso,
                _ when departmentText.ContainsIgnoreCase("строительн") => DepartmentTypeApp.Aso,
                _ when departmentText.Contains("АСО") => DepartmentTypeApp.Aso,
                _ when departmentText.Contains("ОВ") => DepartmentTypeApp.Sto,
                _ when departmentText.Contains("СТО") => DepartmentTypeApp.Sto,
                _ when departmentText.ContainsIgnoreCase("электротехнич") => DepartmentTypeApp.Elto,
                _ when departmentText.Contains("ЭЛТО") => DepartmentTypeApp.Elto,
                _ when departmentText.Contains("ДО") => DepartmentTypeApp.Do,
                _ when departmentText.ContainsIgnoreCase("дорожн") => DepartmentTypeApp.Do,
                _ when departmentText.Contains("ОСиТНГ") => DepartmentTypeApp.OsiTng,
                _ when departmentText.Contains("ТОПНГ") => DepartmentTypeApp.ToPng,
                _ when departmentText.Contains("ТО ПНГ") => DepartmentTypeApp.ToPng,
                _ when departmentText.Contains("ГИП") => DepartmentTypeApp.Gip,
                _ when departmentText.ContainsIgnoreCase("главный инженер") => DepartmentTypeApp.Gip,
                _ when departmentText.ContainsIgnoreCase("директор") => DepartmentTypeApp.Aup,
                _ => DepartmentTypeApp.Unknown,
            };

        /// <summary>
        /// Определен ли тип отдела
        /// </summary>
        public static bool HasDepartmentType(string departmentText) =>
            DepartmentParsing(departmentText) != DepartmentTypeApp.Unknown;
    }
}