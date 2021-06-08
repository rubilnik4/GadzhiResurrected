using System.Collections.Generic;
using GadzhiCommon.Enums.ConvertingSettings;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Models.Interfaces.LibraryData;
using GadzhiResurrected.Modules.GadzhiConvertingModule.Models.Implementations.ProjectSettings;

namespace GadzhiResurrected.Modules.GadzhiConvertingModule.Models.Interfaces.ProjectSettings
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
        /// Типы конвертации
        /// </summary>
        IReadOnlyCollection<ConvertingMode> ConvertingModes { get; }

        /// <summary>
        /// Выбранные типы конвертации
        /// </summary>
        IReadOnlyCollection<ConvertingModeType> ConvertingModesUsed { get; }

        /// <summary>
        /// Использовать подпись по умолчанию для разработчика
        /// </summary>
        bool UseDefaultSignature { get; set; }
    }
}