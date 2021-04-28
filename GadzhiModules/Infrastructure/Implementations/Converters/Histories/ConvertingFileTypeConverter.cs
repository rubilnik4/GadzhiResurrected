using System;
using System.Collections.Generic;
using System.Linq;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiModules.Modules.GadzhiConvertingModule.Models.Enums.Histories;

namespace GadzhiModules.Infrastructure.Implementations.Converters.Histories
{
    /// <summary>
    /// Преобразование типов файлов в строку
    /// </summary>
    public static class ConvertingFileTypeConverter
    {
        /// <summary>
        /// Словарь типа истории в строковом значении
        /// </summary>
        public static IReadOnlyDictionary<ConvertingFileType, string> ConvertingFileTypesString =>
            new Dictionary<ConvertingFileType, string>
            {
                { ConvertingFileType.All, "Все" },
                { ConvertingFileType.Dgn, "Dgn" },
                { ConvertingFileType.Doc, "Doc" },
                { ConvertingFileType.Pdf, "Pdf" },
                { ConvertingFileType.Dwg, "Dwg" },
                { ConvertingFileType.Xls, "Xls" },
                { ConvertingFileType.Print, "Печать" },
            };

        /// <summary>
        /// Типы расширений
        /// </summary>
        public static IReadOnlyCollection<string> FileExtensionsString =>
            ConvertingFileTypesString.Select(fileType => fileType.Value).ToList();

        /// <summary>
        /// Преобразовать тип расширения в наименование
        /// </summary>       
        public static string ConvertingTypeToString(ConvertingFileType convertingFileType)
        {
            ConvertingFileTypesString.TryGetValue(convertingFileType, out string convertingFileString);
            return convertingFileString;
        }

        /// <summary>
        /// Преобразовать наименование расширения в тип
        /// </summary>       
        public static ConvertingFileType ConvertingTypeFromString(string convertingFileType) =>
            ConvertingFileTypesString.FirstOrDefault(convertingFile => convertingFile.Value == convertingFileType).Key;
    }
}