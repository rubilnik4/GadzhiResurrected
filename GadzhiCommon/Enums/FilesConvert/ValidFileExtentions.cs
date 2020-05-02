using System.Collections.Generic;

namespace GadzhiCommon.Enums.FilesConvert
{
    /// <summary>
    /// Список допустимых расширений для конвертации
    /// </summary>
    public static class ValidFileExtentions
    {
        /// <summary>
        /// Список допустимых расширений для конвертации
        /// </summary>
        public static IReadOnlyDictionary<string, FileExtension> DocAndDgnFileTypes => new Dictionary<string, FileExtension>()
        {
            { "doc", FileExtension.Docx},
            { "docx", FileExtension.Docx},
            { "Dgn", FileExtension.Dgn},
        };

        /// <summary>
        /// Список допустимых расширений
        /// </summary>
        public static IReadOnlyDictionary<string, FileExtension> FileTypesValid => new Dictionary<string, FileExtension>()
        {
            { "doc", FileExtension.Docx},
            { "docx", FileExtension.Docx},
            { "Dgn", FileExtension.Dgn},
            { "pdf", FileExtension.Pdf},
            { "dwg", FileExtension.Dwg},
            { "xlsx", FileExtension.Xlsx},
        };
    }
}
