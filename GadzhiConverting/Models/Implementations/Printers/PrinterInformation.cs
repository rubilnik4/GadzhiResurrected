using GadzhiConverting.Models.Interfaces.Printers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GadzhiApplicationCommon.Models.Enums.Printers;

namespace GadzhiConverting.Models.Implementations.Printers
{
    /// <summary>
    /// Информация о принтере
    /// </summary>
    public class PrinterInformation : IPrinterInformation
    {
        public PrinterInformation(string name, PrinterType printerType, PrinterFormatType printerFormatType,
                                  string prefixSearchPaperSize)
        {
            if (String.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));

            Name = name;
            PrinterType = printerType;
            PrinterFormatType = printerFormatType;
            PrefixSearchPaperSize = prefixSearchPaperSize;
        }

        /// <summary>
        /// Имя принтера
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Тип принтера
        /// </summary>
        public PrinterType PrinterType { get; }

        /// <summary>
        /// Тип формата принтера
        /// </summary>
        public PrinterFormatType PrinterFormatType { get; }

        /// <summary>
        /// Параметр поиска форматов печати
        /// </summary>
        public string PrefixSearchPaperSize { get; }
    }
}
