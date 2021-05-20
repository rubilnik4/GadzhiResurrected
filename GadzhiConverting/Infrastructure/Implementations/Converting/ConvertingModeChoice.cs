using System.Collections.Generic;
using System.Linq;
using GadzhiCommon.Enums.FilesConvert;

namespace GadzhiConverting.Infrastructure.Implementations.Converting
{
    /// <summary>
    /// Необходимость конвертации
    /// </summary>
    public static class ConvertingModeChoice
    {
        /// <summary>
        /// Необходимость конвертации PDF
        /// </summary>
        public static bool IsPdfConvertingNeed(IReadOnlyCollection<ConvertingModeType> convertingModeTypes) =>
            convertingModeTypes.Contains(ConvertingModeType.Pdf);

        /// <summary>
        /// Необходимость конвертации DWG
        /// </summary>
        public static bool IsDwgConvertingNeed(IReadOnlyCollection<ConvertingModeType> convertingModeTypes) =>
            convertingModeTypes.Contains(ConvertingModeType.Export);

        /// <summary>
        /// Необходимость печати
        /// </summary>
        public static bool IsPrintConvertingNeed(IReadOnlyCollection<ConvertingModeType> convertingModeTypes) =>
            convertingModeTypes.Contains(ConvertingModeType.Print);
    }
}