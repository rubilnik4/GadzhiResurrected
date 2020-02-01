using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiModules.Infrastructure.Interfaces
{
    /// <summary>
    /// Класс обертка для отлова ошибок
    /// </summary> 
    public interface IExecuteAndCatchErrors
    {
        /// <summary>
        /// Обертка для вызова индикатора загрузки и отлова ошибок метода.
        /// При наличие ошибок WCF останаливает процеес конвертации
        /// </summary> 
        void ExecuteAndHandleError(Action method, Action ApplicationAbortionMethod = null);

        /// <summary>
        /// Обертка для вызова индикатора загрузки и отлова ошибок метода      
        /// </summary> 
        void ExecuteAndHandleError<T1>(Action<T1> function, T1 arg1, Action ApplicationAbortionMethod = null);

        /// <summary>
        /// Обертка для вызова индикатора загрузки и отлова ошибок асинхронного метода
        /// </summary> 
        Task ExecuteAndHandleErrorAsync(Func<Task> asyncMethod, Action ApplicationAbortionMethod = null);

        /// <summary>
        /// Обертка для вызова индикатора загрузки и отлова ошибок асинхронной функции
        /// </summary> 
        Task ExecuteAndHandleErrorAsync<T1>(Func<T1, Task> functionAsync, T1 arg1, Action ApplicationAbortionMethod = null);

    }
}
