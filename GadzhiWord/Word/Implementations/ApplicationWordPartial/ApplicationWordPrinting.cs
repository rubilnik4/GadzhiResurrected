using GadzhiApplicationCommon.Models.Interfaces;
using GadzhiApplicationCommon.Models.Interfaces.ApplicationLibrary.Application;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiMicrostation.Models.Enums;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiWord.Word.Implementations.ApplicationWordPartial
{
    /// <summary>
    /// Печать Word
    /// </summary>
    public partial class ApplicationWord : IApplicationLibraryPrinting
    {
        /// <summary>
        /// Команда печати
        /// </summary>
        public IErrorApplication PrintStamp(IStamp stamp, ColorPrintApplication colorPrint, string prefixSearchPaperSize)
        {
            Application.PrintOut(Range: WdPrintOutRange.wdPrintAllDocument, PageType: WdPrintOutPages.wdPrintAllPages,
                              ManualDuplexPrint: false, PrintToFile: false);
            return null;
        }


    }
}
