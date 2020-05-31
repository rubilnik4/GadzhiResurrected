using System;
using System.Collections.Generic;
using System.Globalization;
using GadzhiApplicationCommon.Extensions.StringAdditional;
using GadzhiMicrostation.Models.Enums;

namespace GadzhiCommon.Enums.FilesConvert
{
    /// <summary>
    /// Список допустимых расширений для конвертации Microstation
    /// </summary>
    public static class ValidMicrostationExtensions
    {
        /// <summary>
        /// Сравнить расширения
        /// </summary>
        public static bool IsFileExtensionEqual(string fileExtension, FileExtensionMicrostation extensionCompare) =>
            extensionCompare.ToString().ToLowerCaseCurrentCulture() == 
            fileExtension.Trim('.').ToLowerCaseCurrentCulture();
    }
}
