using GadzhiConverting.Configuration;
using GadzhiConverting.Models.Implementations;
using System;
using System.Collections.Generic;
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
            foreach (var printers in config.PrintersInformationCollection)
            {
                if (printers is PrintersPdfCollection printersPdf)
                {
                    foreach (PrinterInformationElement printer in printersPdf)
                    {
                        ToPrinterInformation(printer);
                    }
                    Select(printer => );
                }
            }

            return printersInformation;
        }

        /// <summary>
        /// Преобразовать элемент конфигурационного файла в модель о принтере
        /// </summary>       
        private static PrinterInformation ToPrinterInformation(PrinterInformationElement printerInformationElement) =>
             new PrinterInformation(printerInformationElement?.Name);

    }
}
