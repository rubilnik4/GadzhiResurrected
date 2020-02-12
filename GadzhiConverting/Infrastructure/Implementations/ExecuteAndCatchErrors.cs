using GadzhiConverting.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiConverting.Infrastructure.Implementations
{
    /// <summary>
    /// Класс обертка для отлова ошибок
    /// </summary> 
    public class ExecuteAndCatchErrors: IExecuteAndCatchErrors
    {
        /// <summary>
        /// Стандартные диалоговые окна
        /// </summary> 
        private readonly IMessageAndLoggingService _messageAndLoggingService;

        public ExecuteAndCatchErrors(IMessageAndLoggingService messageAndLoggingService)
        {
            _messageAndLoggingService = messageAndLoggingService;
        }

        /// <summary>
        ///Отлов ошибок и вызов постметода       
        /// </summary> 
        public void ExecuteAndHandleError(Action method, 
                                          Action ApplicationFinallyMethod = null)
        {
            try
            {
                method();
            }           
            catch (Exception ex)
            {
                _messageAndLoggingService.ShowMessage(ex.Message + "\n" + ex.StackTrace);
            }    
            finally
            {
                ApplicationFinallyMethod?.Invoke();
            }
        }

        /// <summary>
        ///Отлов ошибок и вызов постметода       
        /// </summary> 
        public void ExecuteAndHandleError<T1>(Action<T1> function, 
                                              T1 arg1, 
                                              Action ApplicationFinallyMethod = null)
        {
            ExecuteAndHandleError(() => function(arg1), ApplicationFinallyMethod);
        }

        /// <summary>
        ///Отлов ошибок и вызов постметода асинхронно     
        /// </summary> 
        public async Task ExecuteAndHandleErrorAsync(Func<Task> asyncMethod, 
                                                     Action ApplicationFinallyMethod = null)
        {            
            try
            {
                await asyncMethod();
            }            
            catch (Exception ex)
            {
                _messageAndLoggingService.ShowMessage(ex.Message + "\n" + ex.StackTrace);
            }
            finally
            {
                ApplicationFinallyMethod?.Invoke();
            }
        }

        /// <summary>
        ///Отлов ошибок и вызов постметода асинхронно     
        /// </summary> 
        public async Task ExecuteAndHandleErrorAsync<T1>(Func<T1, Task> functionAsync, 
                                                         T1 arg1, 
                                                         Action ApplicationFinallyMethod = null)
        {
            await ExecuteAndHandleErrorAsync(() => functionAsync(arg1), ApplicationFinallyMethod);
        }
    }
}
