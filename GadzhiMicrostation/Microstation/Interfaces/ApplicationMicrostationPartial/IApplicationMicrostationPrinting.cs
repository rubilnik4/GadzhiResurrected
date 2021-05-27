using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiMicrostation.Models.Implementations.Coordinates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GadzhiApplicationCommon.Models.Enums.StampCollections;

namespace GadzhiMicrostation.Microstation.Interfaces.ApplicationMicrostationPartial
{
    /// <summary>
    /// Печать Microstation
    /// </summary>
    public interface IApplicationMicrostationPrinting
    {
        /// <summary>
        /// Установить тип поворота
        /// </summary>       
        void SetPrintingOrientation(StampOrientationType orientation);

        /// <summary>
        /// Установить границы печати по рамке
        /// </summary>
        IResultApplication SetPrintingFenceByRange(RangeMicrostation rangeToPrint);

        /// <summary>
        /// Установить формат печати характерный для принтера
        /// </summary>       
        IResultApplication SetPrinterPaperSize(StampPaperSizeType paperSize, string prefixSearchPaperSize);


        /// <summary>
        /// Установить масштаб печати
        /// </summary>       
        void SetPrintScale(double paperScale);

        /// <summary>
        /// Установить цвет печати
        /// </summary>       
        void SetPrintColor(ColorPrintApplication colorPrint);
    }
}
