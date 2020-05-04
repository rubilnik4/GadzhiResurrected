using GadzhiConverting.Models.Interfaces.Printers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiConverting.Models.Implementations.Printers
{
    /// <summary>
    /// Информация о принтере
    /// </summary>
    public class PrinterInformation : IPrinterInformation
    {
        public PrinterInformation(string printerName)
           : this(printerName, null)
        { }

        public PrinterInformation(string name, string prefixSearchPaperSize)
        {
            if (String.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));

            Name = name;
            PrefixSearchPaperSize = prefixSearchPaperSize;
        }

        /// <summary>
        /// Имя принтера
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Параметр поиска форматов печати
        /// </summary>
        public string PrefixSearchPaperSize { get; }
    }
}
