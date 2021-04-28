using System.Collections.Generic;

namespace GadzhiCommon.Infrastructure.Implementations.FilesConvert
{
    /// <summary>
    /// Дополнительные расширения конвертируемых файлов
    /// </summary>
    public static class AdditionalFileExtensions
    {
        /// <summary>
        /// Дополнительные расширения конвертируемых файлов
        /// </summary>
        public static IReadOnlyCollection<string> FileExtensions =>
            new List<string>()
            {
                "jpeg",
                "jpg"
            };
    }
}