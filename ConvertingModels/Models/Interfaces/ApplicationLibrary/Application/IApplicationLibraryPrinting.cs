using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertingModels.Models.Interfaces.ApplicationLibrary.Application
{
    /// <summary>
    /// Печать в приложении
    /// </summary>
    public interface IApplicationLibraryPrinting
    {
        /// <summary>
        /// Команда печати PDF
        /// </summary>
        void PrintCommand();
    }
}
