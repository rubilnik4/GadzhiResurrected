using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiConverting.Models.Interfaces.Printers
{
    /// <summary>
    /// Информация о принтерах
    /// </summary>
    public interface IPrintersInformation
    {       
        /// <summary>
        /// Список принтеров для печати PDF
        /// </summary>
        IEnumerable<IPrinterInformation> PrintersPdf{ get; }
    }
}
