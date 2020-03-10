using ConvertingModels.Models.Interfaces.Printers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertingModels.Models.Interfaces.ApplicationLibrary
{
    /// <summary>
    /// Печать Word
    /// </summary>
    public interface IApplicationLibraryPrinting
    {
        /// <summary>
        /// Команда печати PDF
        /// </summary>
        bool PrintCommand();
    }
}
