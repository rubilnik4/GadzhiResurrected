using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GadzhiCommon.Enums.ConvertingSettings;
using GadzhiCommon.Models.Interfaces.LibraryData;
using GadzhiModules.Modules.GadzhiConvertingModule.Models.Interfaces.ProjectSettings;

namespace GadzhiModules.Modules.GadzhiConvertingModule.Models.Implementations.ProjectSettings
{
    /// <summary>
    /// Параметры конвертации
    /// </summary>
    public class ConvertingSettings : IConvertingSettings
    {
        public ConvertingSettings(ISignatureLibrary personSignature, PdfNamingType pdfNamingType)
        {
            PersonSignature = personSignature ?? throw new ArgumentNullException(nameof(personSignature));
            PdfNamingType = pdfNamingType;
        }

        /// <summary>
        /// Личная подпись
        /// </summary>
        public ISignatureLibrary PersonSignature { get; set; }

        /// <summary>
        /// Принцип именования PDF
        /// </summary>
        public PdfNamingType PdfNamingType { get; set; }
    }
}