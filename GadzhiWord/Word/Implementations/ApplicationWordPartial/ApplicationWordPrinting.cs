using GadzhiApplicationCommon.Models.Interfaces.ApplicationLibrary.Application;
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
        public void PrintStamp() =>       
            Application.PrintOut(Range: WdPrintOutRange.wdPrintAllDocument, PageType: WdPrintOutPages.wdPrintAllPages,
                                 ManualDuplexPrint: false, PrintToFile: false);
       
    }
}
