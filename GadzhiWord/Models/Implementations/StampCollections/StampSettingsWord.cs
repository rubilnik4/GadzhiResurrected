using System;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiApplicationCommon.Models.Implementation.StampCollections;

namespace GadzhiWord.Models.Implementations.StampCollections
{
    /// <summary>
    /// Параметры штампа Word
    /// </summary>
    public class StampSettingsWord: StampSettings
    {
        public StampSettingsWord(StampIdentifier id, string personId, PdfNamingTypeApplication pdfNamingType,
                                 StampPaperSizeType paperSize, StampOrientationType orientationType, bool useDefaultSignature)
            :base(id, personId, pdfNamingType, useDefaultSignature)
        {
            PaperSize = paperSize;
            Orientation = orientationType;
        }

        /// <summary>
        /// Высота строки таблицы
        /// </summary>
        public const decimal HEIGHT_TABLE_ROW = 0.3m;

        /// <summary>
        /// Формат
        /// </summary>
        public StampPaperSizeType PaperSize { get; }

        /// <summary>
        /// Тип расположения штампа
        /// </summary>
        public StampOrientationType Orientation { get; }
    }
}