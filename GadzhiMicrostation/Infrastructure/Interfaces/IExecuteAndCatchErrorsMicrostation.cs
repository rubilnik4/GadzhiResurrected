using GadzhiMicrostation.Models.Enums;
using GadzhiMicrostation.Models.Implementations;
using System;

namespace GadzhiMicrostation.Infrastructure.Interfaces
{
    /// <summary>
    /// Класс обертка для отлова ошибок
    /// </summary> 
    public interface IExecuteAndCatchErrorsMicrostation
    {
        /// <summary>
        ///Отлов ошибок и вызов постметода       
        /// </summary> 
        void ExecuteAndHandleError(Action method,
                                   Action applicationBeforeMethod = null,
                                   Func<ErrorMicrostation> applicationCatchMethod = null,
                                   Action applicationFinallyMethod = null);

    }
}
