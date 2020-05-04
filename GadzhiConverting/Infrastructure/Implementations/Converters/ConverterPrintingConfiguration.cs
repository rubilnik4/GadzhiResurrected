using GadzhiConverting.Configuration;
using GadzhiConverting.Models.Implementations.Printers;
using GadzhiConverting.Models.Interfaces.Printers;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiConverting.Infrastructure.Implementations.Converters
{
    /// <summary>
    /// Заполнить модель с информацией о принтерах
    /// </summary>
    public static class ConverterPrintingConfiguration
    {
        public static IPrintersInformation ToPrintersInformation()
        {           
            var config = RegisterPrintersInformationSection.GetConfig();

            var systemPrinters = PrinterSettings.InstalledPrinters.Cast<string>();
            var printersPdf = config.PrintersPdfCollection.Where(printer => systemPrinters?.
                                                           Contains(printer.Name, StringComparer.OrdinalIgnoreCase) == true).
                                                           Select(ToPrinterInformation);
            return new PrintersInformation(printersPdf);
        }

        /// <summary>
        /// Преобразовать элемент конфигурационного файла в модель о принтере
        /// </summary>       
        private static IPrinterInformation ToPrinterInformation(PrinterInformationElement printerInformationElement) =>
             new PrinterInformation(printerInformationElement?.Name, printerInformationElement?.PrefixSearchPaperSize);
    }
}
