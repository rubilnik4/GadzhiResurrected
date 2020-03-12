using GadzhiCommon.Models.Implementations.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiConverting.Infrastructure.Interfaces
{
    /// <summary>
    /// Управление печатью пдф
    /// </summary>
    public interface IPdfCreatorService : IDisposable
    {  
        /// <summary>
        /// СОздать PDF файл с выполнением отложенной печати 
        /// </summary>       
        (bool Success, ErrorConverting ErrorConverting) PrintPdfWithExecuteAction(string filePath, Action printAction);

    }
}
