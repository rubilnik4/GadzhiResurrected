using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using GadzhiCommon.Enums.LibraryData;
using GadzhiCommon.Extensions.StringAdditional;

namespace GadzhiCommon.Infrastructure.Implementations.Converters.LibraryData
{
    /// <summary>
    /// Операции над типом отдела
    /// </summary>
    public static class ConverterDepartmentType
    {
        /// <summary>
        /// Словарь отделов в строковом значении
        /// </summary>
        public static ImmutableDictionary<DepartmentType, string> DepartmentTypeString =>
            new Dictionary<DepartmentType, string>
            {
                { DepartmentType.Aso , "АСО" },
                { DepartmentType.AsuTp , "АСУТП" },
                { DepartmentType.Aup , "АУП" },
                { DepartmentType.Gip  , "ГИПы" },
                { DepartmentType.Engineer  , "ГМС-Инжиниринг" },
                { DepartmentType.Do , "ДО" },
                { DepartmentType.Ito , "ИТО" },
                { DepartmentType.Koisi , "КОИСИ" },
                { DepartmentType.Nio, "НИО" },
                { DepartmentType.Oan , "ОАН" },
                { DepartmentType.Oisi , "ОИСИ" },
                { DepartmentType.Oisi1 , "ОИСИ-1" },
                { DepartmentType.Oisi2 , "ОИСИ-2" },
                { DepartmentType.Oisi3 , "ОИСИ-3" },
                { DepartmentType.OisiSkr , "ОИСИ-СКР" },
                { DepartmentType.OisiSpo, "ОИСИ-СПО" },
                { DepartmentType.Okia, "ОКиА" },
                { DepartmentType.Opeb, "ОПЭБ" },
                { DepartmentType.OsiTng, "ОСиТНГ" },
                { DepartmentType.So, "СО" },
                { DepartmentType.Sto, "СТО" },
                { DepartmentType.ToPng, "ТОПНГ" },
                { DepartmentType.Elto, "ЭЛТО" },
            }.ToImmutableDictionary();

        /// <summary>
        /// Преобразовать тип отдел в строковое значение
        /// </summary>       
        public static string DepartmentTypeToString(DepartmentType departmentType) =>
            DepartmentTypeString[departmentType];

        /// <summary>
        /// Преобразовать тип отдел в строковое значение или вернуть неизвестный тип
        /// </summary>       
        public static DepartmentType DepartmentStringToTypeOrUnknown(string department) =>
            DepartmentTypeString.ContainsValue(department)
            ? DepartmentStringToType(department)
            : DepartmentType.Unknown;

        /// <summary>
        /// Преобразовать строковое значение отдела в тип
        /// </summary>       
        public static DepartmentType DepartmentStringToType(string department) =>
            !String.IsNullOrWhiteSpace(department)
                ? DepartmentTypeString.ContainsValue(department)
                    ? DepartmentTypeString.FirstOrDefault(departmentPair => departmentPair.Value == department).Key
                    : throw new KeyNotFoundException(nameof(department))
                : throw new ArgumentNullException(nameof(department));
    }
}