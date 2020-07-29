using GadzhiCommon.Models.Implementations.Errors;
using GadzhiCommon.Models.Interfaces.Errors;
using GadzhiConverting.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiConverting.Infrastructure.Interfaces
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
