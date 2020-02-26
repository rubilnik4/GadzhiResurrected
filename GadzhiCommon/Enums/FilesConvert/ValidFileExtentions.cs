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
            { "doc", FileExtension.docx},
            { "docx", FileExtension.docx},
            { "dgn", FileExtension.dgn},           
        };
    }
}
