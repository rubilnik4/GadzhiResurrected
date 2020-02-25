using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiConverting.Models.Implementations
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
        public IList<PrinterInformation> PrintersPdf{ get; set; }
    }
}
