using GadzhiCommon.Models.Interfaces.Errors;
using System;

namespace GadzhiPdfPrinting.Infrastructure.Interfaces
{
    /// <summary>
    /// Управление печатью пдф
    /// </summary>
    public interface IPdfCreatorService 
    {  
        /// <summary>
        /// Создать PDF файл с выполнением отложенной печати 
        /// </summary>       
        IResultError PrintPdfWithExecuteAction(string filePath, Func<IResultError> printFunc);

    }
}
