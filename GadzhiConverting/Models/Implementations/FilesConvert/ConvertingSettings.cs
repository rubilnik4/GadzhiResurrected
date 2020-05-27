using System;
using GadzhiCommon.Enums.ConvertingSettings;
using GadzhiConverting.Models.Interfaces.FilesConvert;

namespace GadzhiConverting.Models.Implementations.FilesConvert
{
    public class ConvertingSettings : IConvertingSettings
    {
        public ConvertingSettings(string personId, PdfNamingType pdfNamingType)
        {
            PersonId = personId ?? String.Empty;
            PdfNamingType = pdfNamingType;
        }

        /// <summary>
        /// Идентификатор личной подпись
        /// </summary>
        public string PersonId { get; }

        /// <summary>
        /// Принцип именование PDF
        /// </summary>
        public PdfNamingType PdfNamingType { get; }
    }
}