using GadzhiApplicationCommon.Models.Interfaces.ApplicationLibrary.Application;
using Microsoft.Office.Interop.Word;

namespace GadzhiWord.Word.Implementations.ApplicationOfficePartial
{
    /// <summary>
    /// Печать Word
    /// </summary>
    public partial class ApplicationOffice : IApplicationLibraryPrinting
    {
        /// <summary>
        /// Команда печати
        /// </summary>
        public void PrintCommand() =>  ApplicationWord.PrintOut(Range: WdPrintOutRange.wdPrintAllDocument, PageType: WdPrintOutPages.wdPrintAllPages,
                                                                ManualDuplexPrint: false, PrintToFile: false);
    }
}
