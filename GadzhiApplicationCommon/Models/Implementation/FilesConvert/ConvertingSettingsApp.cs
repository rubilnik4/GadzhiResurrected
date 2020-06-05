using System;
using GadzhiApplicationCommon.Models.Enums;

namespace GadzhiApplicationCommon.Models.Implementation.FilesConvert
{
    public class ConvertingSettingsApp
    {
        public ConvertingSettingsApp(string personId, PdfNamingTypeApplication pdfNamingType)
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
        public PdfNamingTypeApplication PdfNamingType { get; }
    }
}