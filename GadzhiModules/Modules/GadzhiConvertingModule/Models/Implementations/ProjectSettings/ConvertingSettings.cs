using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GadzhiCommon.Enums.ConvertingSettings;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Infrastructure.Implementations.Logger;
using GadzhiCommon.Models.Interfaces.LibraryData;
using GadzhiModules.Infrastructure.Implementations.Converters;
using GadzhiModules.Modules.GadzhiConvertingModule.Models.Interfaces.ProjectSettings;

namespace GadzhiModules.Modules.GadzhiConvertingModule.Models.Implementations.ProjectSettings
{
    /// <summary>
    /// Параметры конвертации
    /// </summary>
    public class ConvertingSettings : IConvertingSettings
    {
        public ConvertingSettings(ISignatureLibrary personSignature, PdfNamingType pdfNamingType,
                                  ColorPrintType colorPrintType, bool useDefaultSignature)
        {
            PersonSignature = personSignature;
            PdfNamingType = pdfNamingType;
            ColorPrintType = colorPrintType;
            UseDefaultSignature = useDefaultSignature;
        }

        /// <summary>
        /// Личная подпись
        /// </summary>
        [Logger]
        public ISignatureLibrary PersonSignature { get; set; }

        /// <summary>
        /// Принцип именования PDF
        /// </summary>

        [Logger]
        public PdfNamingType PdfNamingType { get; set; }

        /// <summary>
        /// Цвет печати по умолчанию
        /// </summary>
        [Logger]
        public ColorPrintType ColorPrintType { get; set; }

        /// <summary>
        /// Типы конвертации
        /// </summary>
        [Logger]
        public IReadOnlyCollection<ConvertingMode> ConvertingModes { get; } = 
            ConvertingModeTypeConverter.ConvertingModeTypesString.Keys.
            Select(convertingMode => new ConvertingMode(convertingMode,
                                                        ConvertingModeTypeConverter.DefaultConvertingModeTypes.
                                                                                    Contains(convertingMode))).
            ToList();

        /// <summary>
        /// Выбранные типы конвертации
        /// </summary>
        public IReadOnlyCollection<ConvertingModeType> ConvertingModesUsed =>
            ConvertingModes.
            Where(convertingMode => convertingMode.IsUsed).
            Select(convertingMode => convertingMode.ConvertingModeType).
            ToList();

        /// <summary>
        /// Использовать подпись по умолчанию для разработчика
        /// </summary>
        [Logger]
        public bool UseDefaultSignature { get; set; }
    }
}