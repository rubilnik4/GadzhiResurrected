using System;
using GadzhiApplicationCommon.Models.Interfaces.Errors;

namespace GadzhiApplicationCommon.Infrastructure.Interfaces
{
    /// <summary>
    /// Управление печатью пдф
    /// </summary>
    public interface IPdfCreatorService 
    {  
        /// <summary>
        /// Создать PDF файл с выполнением отложенной печати 
        /// </summary>       
        IResultApplication PrintPdfWithExecuteAction(string filePath, Func<IResultApplication> printFunc);

    }
}
