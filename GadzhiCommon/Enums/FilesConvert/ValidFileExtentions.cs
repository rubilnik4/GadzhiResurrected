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
        public static IReadOnlyList<string> DocAndDgnFileTypes => new List<string>()
        {
            "doc",
            "docx",
            "dgn",
        };
    }
}
