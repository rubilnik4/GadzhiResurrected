using System;
using System.Linq;
using GadzhiApplicationCommon.Extensions.Functional;
using GadzhiApplicationCommon.Extensions.StringAdditional;
using GadzhiApplicationCommon.Models.Enums.StampCollections;

namespace GadzhiApplicationCommon.Infrastructure.Implementations.Converters.StampCollections
{
    /// <summary>
    /// Преобразование формата печати
    /// </summary>
    public static class ConverterStampPaperSize
    {
        /// <summary>
        /// Преобразовать формата печати
        /// </summary>
        public static StampPaperSizeType ToPaperSize(string paperSize) =>
            paperSize.Replace('х', 'x').Trim().
            Map(paperSizeFormat => Enum.GetValues(typeof(StampPaperSizeType)).
                                   Cast<StampPaperSizeType>().
                                   Select(stampPaperSizeType => stampPaperSizeType.ToString()).
                                   Any(stampPaperSizeType => stampPaperSizeType.ContainsIgnoreCase(paperSizeFormat))
                                        ? (StampPaperSizeType)Enum.Parse(typeof(StampPaperSizeType), paperSizeFormat, true)
                                        : StampPaperSizeType.Undefined);
    }
}