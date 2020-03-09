using ConvertingModels.Models.Interfaces.Printers;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiWord.Helpers.Implementations;
using GadzhiWord.Word.Interfaces.ApplicationWordPartial;
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
    public partial class ApplicationWord : IApplicationWordPrinting
    {
        /// <summary>
        /// Установить принтер по умолчанию
        /// </summary>       
        public bool SetDefaultPrinter(IPrinterInformation printerInformation)
        {
            bool success = false;
            if (PrinterSettings.InstalledPrinters?.Cast<string>()?.Contains(printerInformation?.Name,
                                                                          StringComparer.OrdinalIgnoreCase) == true)
            {
                if (NativeMethods.SetDefaultPrinter(printerInformation?.Name))
                {
                    success = true;
                }
                else
                {
                    _messagingService.ShowAndLogError(new ErrorConverting(FileConvertErrorType.PrinterNotInstall,
                                                                      $"Не удалось установить принтер {printerInformation?.Name}"));
                }
            }
            else
            {
                _messagingService.ShowAndLogError(new ErrorConverting(FileConvertErrorType.PrinterNotInstall,
                                                                        $"Принтер {printerInformation?.Name} не установлен в системе"));
            }

            return success;
        }

        /// <summary>
        /// Установить принтер PDF по умолчанию
        /// </summary>       
        public string SetDefaultPdfPrinter()
        {
            IPrinterInformation pdfPrinterInformation = _wordProject.PrintersInformation.PrintersPdf.FirstOrDefault();
            SetDefaultPrinter(pdfPrinterInformation);
            return pdfPrinterInformation.Name;
        }


        /// <summary>
        /// Команда печати
        /// </summary>
        public void PrintCommand()
        {
            _application.PrintOut(Range: WdPrintOutRange.wdPrintAllDocument, PageType: WdPrintOutPages.wdPrintAllPages,
                                  ManualDuplexPrint: false, PrintToFile: false);
        }

        /// <summary>
        /// Команда печати PDF
        /// </summary>
        public bool PrintPdfCommand(string filePath) => _pdfCreatorService.PrintPdfWithExecuteAction(filePath, PrintCommand);

    }
}
