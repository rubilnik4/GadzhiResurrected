using System;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Implementation.FilesConvert;

namespace GadzhiApplicationCommon.Models.Implementation.StampCollections
{
    /// <summary>
    /// 
    /// </summary>
    public class StampSettings: ConvertingSettingsApplication
    {
        public StampSettings(StampIdentifier id, string personId, PdfNamingTypeApplication pdfNamingType)
            :base(personId, pdfNamingType)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
        }

        /// <summary>
        /// Идентификатор штампа
        /// </summary>
        public StampIdentifier Id { get; }
    }
}