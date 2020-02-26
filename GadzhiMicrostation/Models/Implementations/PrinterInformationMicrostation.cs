using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Models.Implementations
{
    /// <summary>
    /// Параметры принтера
    /// </summary>
    public class PrinterInformationMicrostation
    {
        public PrinterInformationMicrostation(string printerName)
            : this(printerName, null)
        {
        }

        public PrinterInformationMicrostation(string printerName, string prefixSearchPaperSize)
        {
            if (!String.IsNullOrEmpty (printerName))
            {
                PrinterName = printerName;
                PrefixSearchPaperSize = prefixSearchPaperSize;
            }
            else
            {
                throw new ArgumentNullException(nameof(printerName));
            }
        }

        /// <summary>
        /// Имя принтера
        /// </summary>
        public string PrinterName { get; }

        /// <summary>
        /// Параметр поиска форматов печати
        /// </summary>
        public string PrefixSearchPaperSize { get; }
    }
}
