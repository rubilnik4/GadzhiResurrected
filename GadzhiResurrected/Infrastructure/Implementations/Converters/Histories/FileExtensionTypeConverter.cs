using System.Collections.Generic;
using GadzhiCommon.Enums.FilesConvert;

namespace GadzhiResurrected.Infrastructure.Implementations.Converters.Histories
{
    /// <summary>
    /// Преобразование типа файла
    /// </summary>
    public static class FileExtensionTypeConverter
    {
        /// <summary>
        /// Словарь типа истории в строковом значении
        /// </summary>
        public static IReadOnlyDictionary<FileExtensionType, string> FileExtensionTypesString =>
            new Dictionary<FileExtensionType, string>
            {
                {FileExtensionType.Dgn, "Dgn"},
                {FileExtensionType.Doc, "Doc"},
                {FileExtensionType.Docx, "Docx"},
                {FileExtensionType.Pdf, "Pdf"},
                {FileExtensionType.Dwg, "Dwg"},
                {FileExtensionType.Xls, "Xls"},
                {FileExtensionType.Xlsx, "Xlsx"},
                {FileExtensionType.All, "Все"},
                {FileExtensionType.Print, "Печать"},
                {FileExtensionType.Undefined, "Не определено"},
            };

        /// <summary>
        /// Преобразовать в строку
        /// </summary>
        public static string FileExtensionToString(FileExtensionType fileExtensionType)
        {
            FileExtensionTypesString.TryGetValue(fileExtensionType, out string fileExtensionTypeString);
            return fileExtensionTypeString;
        }
    }
}