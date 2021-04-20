using GadzhiCommon.Enums.ConvertingSettings;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Models.Interfaces.LibraryData;

namespace GadzhiModules.Modules.GadzhiConvertingModule.Models.Interfaces.ProjectSettings
{
    /// <summary>
    /// Параметры конвертации
    /// </summary>
    public interface IConvertingSettings
    {
        /// <summary>
        /// Личная подпись
        /// </summary>
        ISignatureLibrary PersonSignature { get; set; }

        /// <summary>
        /// Принцип именования PDF
        /// </summary>
        PdfNamingType PdfNamingType { get; set; }

        /// <summary>
        /// Цвет печати по умолчанию
        /// </summary>
        ColorPrintType ColorPrintType { get; set; }

        /// <summary>
        /// Тип конвертации
        /// </summary>
        ConvertingModeType ConvertingModeType { get; set; }
    }
}