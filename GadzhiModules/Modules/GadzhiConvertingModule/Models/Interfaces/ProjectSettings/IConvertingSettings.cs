using System.Collections.Generic;
using System.Linq;
using GadzhiCommon.Enums.ConvertingSettings;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Infrastructure.Implementations.Logger;
using GadzhiCommon.Models.Interfaces.LibraryData;
using GadzhiModules.Infrastructure.Implementations.Converters;
using GadzhiModules.Modules.GadzhiConvertingModule.Models.Implementations.ProjectSettings;

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