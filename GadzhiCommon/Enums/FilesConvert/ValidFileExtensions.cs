using System;
using System.Collections.Generic;
using System.Globalization;
using GadzhiCommon.Extensions.StringAdditional;
using GadzhiCommon.Infrastructure.Implementations;

namespace GadzhiCommon.Enums.FilesConvert
{
    /// <summary>
    /// Список допустимых расширений для конвертации
    /// </summary>
    public static class ValidFileExtensions
    {
        /// <summary>
        /// Список допустимых расширений для конвертации
        /// </summary>
        public static IReadOnlyDictionary<string, FileExtension> DocAndDgnFileTypeDictionary => new Dictionary<string, FileExtension>()
        {
            { "doc", FileExtension.Docx},
            { "docx", FileExtension.Docx},
            { "dgn", FileExtension.Dgn},
        };

        /// <summary>
        /// Список допустимых расширений
        /// </summary>
        public static IReadOnlyDictionary<string, FileExtension> FileTypesValidDictionary => new Dictionary<string, FileExtension>()
        {
            { "doc", FileExtension.Docx},
            { "docx", FileExtension.Docx},
            { "dgn", FileExtension.Dgn},
            { "pdf", FileExtension.Pdf},
            { "dwg", FileExtension.Dwg},
            { "xlsx", FileExtension.Xlsx},
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
        public static FileExtension GetDocAndDgnFileTypes(string extension) =>
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
        public static FileExtension GetFileTypesValid(string extension) =>
            FileTypesValidDictionary[extension.ToLowerCaseCurrentCulture()];

        /// <summary>
        /// Сравнить расширения
        /// </summary>
        public static bool IsFileExtensionEqual(string fileExtension, FileExtension extensionCompare) =>
            extensionCompare.ToString().ToLowerCaseCurrentCulture() == 
            FileSystemOperations.ExtensionWithoutPoint(fileExtension).ToLowerCaseCurrentCulture();
    }
}
