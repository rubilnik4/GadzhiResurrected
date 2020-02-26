using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Models.Implementations
{
    /// <summary>
    /// Список используемых принтеров
    /// </summary>
    public class PrintersInformationMicrostation
    {
        public PrintersInformationMicrostation(PrinterInformationMicrostation pdfPrinter)
        {
            PdfPrinter = pdfPrinter;
        }

        /// <summary>
        /// Принтер для печати PDF
        /// </summary>
        public PrinterInformationMicrostation PdfPrinter { get; }
    }
}
