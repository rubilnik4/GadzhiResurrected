using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GadzhiCommon.Enums.ConvertingSettings;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Infrastructure.Implementations.Logger;
using GadzhiCommon.Models.Interfaces.LibraryData;
using GadzhiModules.Modules.GadzhiConvertingModule.Models.Interfaces.ProjectSettings;

namespace GadzhiModules.Modules.GadzhiConvertingModule.Models.Implementations.ProjectSettings
{
    /// <summary>
    /// Параметры конвертации
    /// </summary>
    public class ConvertingSettings : IConvertingSettings
    {
        public ConvertingSettings(ISignatureLibrary personSignature, PdfNamingType pdfNamingType,
                                  ColorPrintType colorPrintType, ConvertingModeType convertingModeType,
                                  bool useDefaultSignature)
        {
            PersonSignature = personSignature ?? throw new ArgumentNullException(nameof(personSignature));
            PdfNamingType = pdfNamingType;
            ColorPrintType = colorPrintType;
            ConvertingModeType = convertingModeType;
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
        /// Тип конвертации
        /// </summary>
        [Logger]
        public ConvertingModeType ConvertingModeType { get; set; }

        /// <summary>
        /// Использовать подпись по умолчанию для разработчика
        /// </summary>
        [Logger]
        public bool UseDefaultSignature { get; set; }
    }
}