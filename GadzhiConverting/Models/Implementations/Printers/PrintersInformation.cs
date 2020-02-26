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
    public class PrintersInformation
    {
        public PrintersInformation()
        {
            PrintersPdf =new List<PrinterInformation>();
        }

        /// <summary>
        /// Список принтеров для печати PDF
        /// </summary>
        public IEnumerable<PrinterInformation> PrintersPdf{ get; set; }
    }
}
