using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
