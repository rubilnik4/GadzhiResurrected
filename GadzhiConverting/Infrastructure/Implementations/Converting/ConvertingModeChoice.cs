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
        public static bool IsPdfConvertingNeed(ConvertingModeType convertingModeType) =>
            convertingModeType == ConvertingModeType.All || convertingModeType == ConvertingModeType.Pdf;

        /// <summary>
        /// Необходимость конвертации DWG
        /// </summary>
        public static bool IsDwgConvertingNeed(ConvertingModeType convertingModeType) =>
            convertingModeType == ConvertingModeType.All || convertingModeType == ConvertingModeType.Export;
    }
}