using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GadzhiApplicationCommon.Models.Enums.Printers;

namespace GadzhiConverting.Models.Interfaces.Printers
{
    /// <summary>
    /// Информация о принтере
    /// </summary>
    public interface IPrinterInformation
    {
        /// <summary>
        /// Имя принтера
        /// </summary>
        string Name { get; }


        /// <summary>
        /// Тип принтера
        /// </summary>
        PrinterType PrinterType { get; }

        /// <summary>
        /// Тип формата принтера
        /// </summary>
        PrinterFormatType PrinterFormatType { get; }

        /// <summary>
        /// Параметр поиска форматов печати
        /// </summary>
        string PrefixSearchPaperSize { get; }
    }
}
