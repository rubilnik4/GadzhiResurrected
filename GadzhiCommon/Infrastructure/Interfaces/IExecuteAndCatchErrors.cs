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
                                   Action ApplicationBeforeMethod = null,
                                   Action ApplicationCatchMethod = null,
                                   Action ApplicationFinallyMethod = null);

        /// <summary>
        ///Отлов ошибок и вызов постметода асинхронно     
        /// </summary> 
        Task ExecuteAndHandleErrorAsync(Func<Task> asyncMethod,
                                        Action ApplicationBeforeMethod = null,
                                        Action ApplicationCatchMethod = null,
                                        Action ApplicationFinallyMethod = null);
    }
}
