using ConvertingModels.Models.Interfaces.Printers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiWord.Word.Interfaces.ApplicationWordPartial
{
    /// <summary>
    /// Печать Word
    /// </summary>
    public interface IApplicationWordPrinting
    {
        /// <summary>
        /// Установить принтер по умолчанию
        /// </summary>       
        bool SetDefaultPrinter(IPrinterInformation printerInformation);

        /// <summary>
        /// Установить принтер PDF по умолчанию
        /// </summary>       
        string SetDefaultPdfPrinter();

        /// <summary>
        /// Команда печати PDF
        /// </summary>
        bool PrintPdfCommand(string filePath);
    }
}
