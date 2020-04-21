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
        public static IReadOnlyDictionary<string, FileExtention> DocAndDgnFileTypes => new Dictionary<string, FileExtention>()
        {
            { "doc", FileExtention.docx},
            { "docx", FileExtention.docx},
            { "dgn", FileExtention.dgn},
        };

        /// <summary>
        /// Список допустимых расширений
        /// </summary>
        public static IReadOnlyDictionary<string, FileExtention> FileTypesValid => new Dictionary<string, FileExtention>()
        {
            { "doc", FileExtention.docx},
            { "docx", FileExtention.docx},
            { "dgn", FileExtention.dgn},
            { "pdf", FileExtention.pdf},
            { "dwg", FileExtention.dwg},
            { "xlsx", FileExtention.xlsx},
        };
    }
}
