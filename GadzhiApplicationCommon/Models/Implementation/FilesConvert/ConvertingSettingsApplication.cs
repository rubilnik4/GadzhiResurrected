using System;
using GadzhiApplicationCommon.Models.Enums;

namespace GadzhiApplicationCommon.Models.Implementation.FilesConvert
{
    public class ConvertingSettingsApplication
    {
        public ConvertingSettingsApplication(string personId, PdfNamingTypeApplication pdfNamingType)
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