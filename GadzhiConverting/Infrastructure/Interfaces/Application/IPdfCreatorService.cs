using GadzhiCommon.Models.Implementations.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiConverting.Infrastructure.Interfaces.Application
{
    /// <summary>
    /// Управление печатью пдф
    /// </summary>
    public interface IPdfCreatorService : IDisposable
    {
        /// <summary>
        /// Установить опции печати
        /// </summary>   
        bool SetPrinterOptions(string filePath);

        /// <summary>
        /// Напечатать PDF
        /// </summary>
        bool PrintPdf();

        /// <summary>
        /// СОздать PDF файл с выполнением отложенной печати 
        /// </summary>       
        (bool Success, IEnumerable<ErrorConverting> ErrorsConverting) PrintPdfWithExecuteAction(string filePath, Action printAction);

    }
}
