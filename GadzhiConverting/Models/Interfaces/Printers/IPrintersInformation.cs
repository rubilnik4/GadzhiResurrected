using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GadzhiCommon.Models.Interfaces.Errors;

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
        IEnumerable<IPrinterInformation> Printers { get; }

        /// <summary>
        /// Принтер PDF
        /// </summary>
        IResultValue<IPrinterInformation> PdfPrinter { get; }

        /// <summary>
        /// Принтер больших форматов черно-белые
        /// </summary>
        IResultValue<IPrinterInformation> BigBlackPrinter { get; }

        /// <summary>
        /// Принтер малых форматов черно-белые
        /// </summary>
        IResultValue<IPrinterInformation> SmallBlackPrinter { get; }
    }
}
