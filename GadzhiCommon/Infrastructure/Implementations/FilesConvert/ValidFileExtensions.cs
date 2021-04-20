using System;
using System.Collections.Generic;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Extensions.StringAdditional;

namespace GadzhiCommon.Infrastructure.Implementations.FilesConvert
{
    /// <summary>
    /// Список допустимых расширений для конвертации
    /// </summary>
    public static class ValidFileExtensions
    {
        /// <summary>
        /// Список допустимых расширений для конвертации
        /// </summary>
        public static IReadOnlyDictionary<string, FileExtensionType> DocAndDgnFileTypeDictionary => new Dictionary<string, FileExtensionType>()
        {
            { "doc", FileExtensionType.Doc},
            { "docx", FileExtensionType.Docx},
            { "dgn", FileExtensionType.Dgn},
        };

        /// <summary>
        /// Список допустимых расширений
        /// </summary>
        public static IReadOnlyDictionary<string, FileExtensionType> FileTypesValidDictionary => new Dictionary<string, FileExtensionType>()
        {
            { "doc", FileExtensionType.Doc},
            { "docx", FileExtensionType.Docx},
            { "dgn", FileExtensionType.Dgn},
            { "pdf", FileExtensionType.Pdf},
            { "dwg", FileExtensionType.Dwg},
            { "xlsx", FileExtensionType.Xlsx},
        };

        /// <summary>
        /// Содержится ли расширение в списке допустимых для конвертации
        /// </summary>
        public static bool ContainsInDocAndDgnFileTypes(string extension) =>
            !String.IsNullOrWhiteSpace(extension) &&
            DocAndDgnFileTypeDictionary.ContainsKey(extension.ToLowerCaseCurrentCulture());

        /// <summary>
        /// Получить допустимое расширение для конвертации
        /// </summary>
        public static FileExtensionType GetDocAndDgnFileTypes(string extension) =>
            DocAndDgnFileTypeDictionary[extension.ToLowerCaseCurrentCulture()];

        /// <summary>
        /// Содержится ли расширение в списке допустимых для конвертации
        /// </summary>
        public static bool ContainsInFileTypesValid(string extension) =>
            !String.IsNullOrWhiteSpace(extension) &&
            FileTypesValidDictionary.ContainsKey(extension.ToLowerCaseCurrentCulture());

        /// <summary>
        /// Получить допустимое расширение для конвертации
        /// </summary>
        public static FileExtensionType GetFileTypesValid(string extension) =>
            FileTypesValidDictionary[extension.ToLowerCaseCurrentCulture()];

        /// <summary>
        /// Сравнить расширения
        /// </summary>
        public static bool IsFileExtensionEqual(string fileExtension, FileExtensionType extensionTypeCompare) =>
            extensionTypeCompare.ToString().ToLowerCaseCurrentCulture() == 
            FileSystemOperations.ExtensionWithoutPoint(fileExtension).ToLowerCaseCurrentCulture();
    }
}
