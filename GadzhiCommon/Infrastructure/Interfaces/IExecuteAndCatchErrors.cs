using GadzhiCommon.Models.Interfaces.Errors;
using System;
using System.Threading.Tasks;

namespace GadzhiCommon.Infrastructure.Interfaces
{
    /// <summary>
    /// Класс обертка для отлова ошибок
    /// </summary> 
    public interface IExecuteAndCatchErrors
    {
        /// <summary>
        ///Отлов ошибок и вызов постметода       
        /// </summary> 
        void ExecuteAndHandleError(Action method,
                                   Action applicationBeforeMethod = null,
                                   Func<IErrorConverting> applicationCatchMethod = null,
                                   Action applicationFinallyMethod = null);

        /// <summary>
        ///Отлов ошибок и вызов постметода асинхронно     
        /// </summary> 
        Task ExecuteAndHandleErrorAsync(Func<Task> asyncMethod,
                                        Action applicationBeforeMethod = null,
                                        Func<Task<IErrorConverting>> applicationCatchMethod = null,
                                        Action applicationFinallyMethod = null);
    }
}
