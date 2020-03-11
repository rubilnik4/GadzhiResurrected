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
        IErrorConverting ExecuteAndHandleError(Action method,
                                   Action applicationBeforeMethod = null,
                                   Action applicationCatchMethod = null,
                                   Action applicationFinallyMethod = null);

        /// <summary>
        ///Отлов ошибок и вызов постметода асинхронно     
        /// </summary> 
        Task<IErrorConverting> ExecuteAndHandleErrorAsync(Func<Task> asyncMethod,
                                        Action applicationBeforeMethod = null,
                                        Action applicationCatchMethod = null,
                                        Action applicationFinallyMethod = null);
    }
}
