using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertingModels.Models.Interfaces.Printers
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
        /// Параметр поиска форматов печати
        /// </summary>
        string PrefixSearchPaperSize { get; }
    }
}
