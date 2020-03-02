using ConvertingModels.Models.Interfaces.Printers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiConverting.Models.Implementations.Printers
{
    /// <summary>
    /// Информация о принтерах
    /// </summary>
    public class PrintersInformation: IPrintersInformation
    {
        public PrintersInformation()
        {
            PrintersPdf =new List<PrinterInformation>();
        }

        /// <summary>
        /// Список принтеров для печати PDF
        /// </summary>
        public IEnumerable<IPrinterInformation> PrintersPdf{ get; set; }
    }
}
