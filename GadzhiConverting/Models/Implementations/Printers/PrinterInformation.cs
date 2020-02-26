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
    public class PrinterInformation
    {
        public PrinterInformation(string printerName)
           : this(printerName, null)
        {
        }

        public PrinterInformation(string name, string prefixSearchPaperSize)
        {
            if (!String.IsNullOrWhiteSpace(name))
            {
                Name = name;
                PrefixSearchPaperSize = prefixSearchPaperSize;
            }
            else
            {
                throw new ArgumentNullException(nameof(name));
            }
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
