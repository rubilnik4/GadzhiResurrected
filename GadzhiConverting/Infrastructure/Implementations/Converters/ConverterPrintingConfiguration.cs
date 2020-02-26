using GadzhiConverting.Configuration;
using GadzhiConverting.Models.Implementations;
using GadzhiConverting.Models.Implementations.Printers;
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
        public static PrintersInformation ToPrintersInformation()
        {
            var printersInformation = new PrintersInformation();

            var config = RegisterPrintersInformationSection.GetConfig();

            var systemPrinters = PrinterSettings.InstalledPrinters.Cast<string>();
           
            return new PrintersInformation()
            {
                PrintersPdf = config.PrintersPdfCollection.Where(printer => systemPrinters?.Contains(printer.Name, StringComparer.OrdinalIgnoreCase) == true).
                                                           Select(printer => ToPrinterInformation(printer)),
            };
        }

        /// <summary>
        /// Преобразовать элемент конфигурационного файла в модель о принтере
        /// </summary>       
        private static PrinterInformation ToPrinterInformation(PrinterInformationElement printerInformationElement) =>
             new PrinterInformation(printerInformationElement?.Name, printerInformationElement?.PrefixSearchPaperSize);

    }
}
