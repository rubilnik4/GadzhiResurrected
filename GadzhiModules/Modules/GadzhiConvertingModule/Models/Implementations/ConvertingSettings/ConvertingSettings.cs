using System;
using System.Collections.Generic;
using GadzhiCommon.Enums.ConvertingSettings;
using GadzhiCommon.Models.Interfaces.LibraryData;
using GadzhiModules.Modules.GadzhiConvertingModule.Models.Interfaces.ConvertingSettings;

namespace GadzhiModules.Modules.GadzhiConvertingModule.Models.Implementations.ConvertingSettings
{
    /// <summary>
    /// Параметры конвертации, отображение
    /// </summary>
    public class ConvertingSettings : IConvertingSettings
    {
        public ConvertingSettings()
            : this(null, String.Empty,  PdfNamingType.ByFile)
        { }

        public ConvertingSettings( ISignatureLibrary personSignature, string department, PdfNamingType pdfNamingType)
        {
            Department = department ?? String.Empty;
            PersonSignature = personSignature;
            PdfNamingType = pdfNamingType;
        }

        /// <summary>
        /// Личная подпись
        /// </summary>
        public ISignatureLibrary PersonSignature { get; set; }

        /// <summary>
        /// Отдел
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        /// Принцип именования PDF
        /// </summary>
        public PdfNamingType PdfNamingType { get; set; }
    }
}