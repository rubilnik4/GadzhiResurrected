using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiConverting.Infrastructure.Interfaces
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
                                   Action ApplicationFinallyMethod = null);

        /// <summary>
        ///Отлов ошибок и вызов постметода       
        /// </summary> 
        void ExecuteAndHandleError<T1>(Action<T1> function,
                                       T1 arg1,
                                       Action ApplicationFinallyMethod = null);

        /// <summary>
        ///Отлов ошибок и вызов постметода асинхронно     
        /// </summary> 
        Task ExecuteAndHandleErrorAsync(Func<Task> asyncMethod,
                                        Action ApplicationFinallyMethod = null);

        /// <summary>
        ///Отлов ошибок и вызов постметода асинхронно     
        /// </summary> 
        Task ExecuteAndHandleErrorAsync<T1>(Func<T1, Task> functionAsync,
                                            T1 arg1,
                                            Action ApplicationAbortionMethod = null);

    }
}
