using GadzhiMicrostation.Models.Coordinates;
using GadzhiMicrostation.Models.Enums;
using GadzhiMicrostation.Models.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Microstation.Interfaces.ApplicationMicrostationPartial
{
    /// <summary>
    /// Печать Microstation
    /// </summary>
    public interface IApplicationMicrostationPrinting
    {
        /// <summary>
        /// Установить принтер по умолчанию
        /// </summary>       
        bool SetDefaultPrinter(PrinterInformationMicrostation printerInformation);

        /// <summary>
        /// Установить тип поворота
        /// </summary>       
        void SetPrintingOrientation(OrientationType orientation);

        /// <summary>
        /// Установить границы печати по рамке
        /// </summary>
        bool SetPrintingFenceByRange(RangeMicrostation rangeToPrint);

        /// <summary>
        /// SetPrinterPaperSize формат печати характерный для принтера
        /// </summary>       
        bool SetPrinterPaperSize(string drawPaperSize, string prefixSearchPaperSize);

        /// <summary>
        /// Установить масштаб печати
        /// </summary>       
        void SetPrintScale(double paperScale);

        /// <summary>
        /// Установить цвет печати
        /// </summary>       
        void SetPrintColor(ColorPrintMicrostation colorPrint);

        /// <summary>
        /// Команда печати
        /// </summary>
        void PrintCommand();

        /// <summary>
        /// Команда печати PDF
        /// </summary>
        bool PrintPdfCommand(string filePath);
    }
}
