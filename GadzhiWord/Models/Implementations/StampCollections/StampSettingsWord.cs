using System;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Implementation.StampCollections;

namespace GadzhiWord.Models.Implementations.StampCollections
{
    public class StampSettingsWord: StampSettings
    {
        public StampSettingsWord(StampIdentifier id, string department, string paperSize, OrientationType orientationType)
            :base(id, department)
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
        public OrientationType Orientation { get; }
    }
}