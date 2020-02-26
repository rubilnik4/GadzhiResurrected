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
        public static IReadOnlyDictionary<string, FileExtensions> DocAndDgnFileTypes => new Dictionary<string, FileExtensions>()
        {
            { "doc", FileExtensions.docx},
            { "docx", FileExtensions.docx},
            { "dgn", FileExtensions.dgn},           
        };
    }
}
