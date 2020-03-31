using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiMicrostation.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiApplicationCommon.Models.Interfaces.ApplicationLibrary.Application
{
    /// <summary>
    /// Печать в приложении
    /// </summary>
    public interface IApplicationLibraryPrinting
    {
        /// <summary>
        /// Команда печати PDF
        /// </summary>
        IEnumerable<IErrorApplication> PrintStamp(IStamp stamp, ColorPrintApplication colorPrint, string prefixSearchPaperSize);
    }
}
