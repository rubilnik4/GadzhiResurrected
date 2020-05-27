using System;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiApplicationCommon.Models.Implementation.StampCollections;

namespace GadzhiWord.Models.Implementations.StampCollections
{
    public class StampSettingsWord: StampSettings
    {
        public StampSettingsWord(StampIdentifier id, string personId, PdfNamingTypeApplication pdfNamingType, 
                                 string paperSize, StampOrientationType orientationType)
            :base(id, personId, pdfNamingType)
        {
            PaperSize = !String.IsNullOrWhiteSpace(paperSize)
                      ? paperSize
                      : throw new ArgumentNullException(nameof(paperSize));
            Orientation = orientationType;
        }

        /// <summary>
        /// Формат
        /// </summary>
        public string PaperSize { get; }

        /// <summary>
        /// Тип расположения штампа
        /// </summary>
        public StampOrientationType Orientation { get; }
    }
}