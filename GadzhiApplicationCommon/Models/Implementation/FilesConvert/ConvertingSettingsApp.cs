using System;
using GadzhiApplicationCommon.Models.Enums;

namespace GadzhiApplicationCommon.Models.Implementation.FilesConvert
{
    public class ConvertingSettingsApp
    {
        public ConvertingSettingsApp(string personId, PdfNamingTypeApplication pdfNamingType, bool useDefaultSignature)
        {
            PersonId = personId ?? String.Empty;
            PdfNamingType = pdfNamingType;
            UseDefaultSignature = useDefaultSignature;
        }

        /// <summary>
        /// Идентификатор личной подписи
        /// </summary>
        public string PersonId { get; }

        /// <summary>
        /// Принцип именование PDF
        /// </summary>
        public PdfNamingTypeApplication PdfNamingType { get; }

        /// <summary>
        /// Использовать подпись по умолчанию
        /// </summary>
        public bool UseDefaultSignature { get; }
    }
}