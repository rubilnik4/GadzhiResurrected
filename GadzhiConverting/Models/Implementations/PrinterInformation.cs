using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiConverting.Models.Implementations
{
    /// <summary>
    /// Информация о принтере
    /// </summary>
    public class PrinterInformation
    {
        public PrinterInformation(string name)
        {
            if (!String.IsNullOrWhiteSpace(name))
            {
                Name = name;
            }
            else
            {
                throw new ArgumentNullException(nameof(name));
            }
        }

        /// <summary>
        /// Имя принтера
        /// </summary>
        public string Name { get; }
    }
}
