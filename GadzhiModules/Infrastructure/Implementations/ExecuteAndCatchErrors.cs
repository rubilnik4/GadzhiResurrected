using GadzhiModules.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiModules.Infrastructure.Implementations
{
    /// <summary>
    /// Класс обертка для отлова ошибок
    /// </summary> 
    public class ExecuteAndCatchErrors: IExecuteAndCatchErrors
    {
        /// <summary>
        /// Стандартные диалоговые окна
        /// </summary> 
        private IDialogServiceStandard DialogServiceStandard { get; set; }

        public ExecuteAndCatchErrors(IDialogServiceStandard dialogServiceStandard)
        {
            DialogServiceStandard = dialogServiceStandard;
        }
        /// <summary>
        /// Обертка для вызова индикатора загрузки и отлова ошибок метода.
        /// При наличие ошибок WCF останаливает процесс конвертации
        /// </summary> 
        public void ExecuteAndHandleError(Action method, Action ApplicationAbortionMethod = null)
        {
            try
            {
                method();
            }
            //// Ошибки WCF сервера
            //catch (TimeoutException ex)
            //{
            //    ApplicationAbortionMethod?.Invoke();
            //    DialogServiceStandard.ShowMessage(ex.Message);
            //}
            //catch (CommunicationException ex)
            //{
            //    ApplicationAbortionMethod?.Invoke();
            //    DialogServiceStandard.ShowMessage(ex.Message);
            //}
            // Общие ошибки
            catch (Exception ex)
            {
                ApplicationAbortionMethod?.Invoke();
                DialogServiceStandard.ShowMessage(ex.Message);
            }           
        }

        public void ExecuteAndHandleError<T1>(Action<T1> function, T1 arg1, Action ApplicationAbortionMethod = null)
        {
            ExecuteAndHandleError(() => function(arg1), ApplicationAbortionMethod);
        }

        //https://gist.github.com/ghstahl/7022ee06c1f9a1753a11efb51882740c
        /// <summary>
        /// Обертка для вызова индикатора загрузки и отлова ошибок асинхронного метода
        /// </summary> 
        public async Task ExecuteAndHandleErrorAsync(Func<Task> asyncMethod, Action ApplicationAbortionMethod = null)
        {            
            try
            {
                await asyncMethod();
            }
            //// Ошибки WCF сервера
            //catch (TimeoutException ex)
            //{
            //    DialogServiceStandard.ShowMessage(ex.Message);
            //}
            //catch (CommunicationException ex)
            //{
            //    DialogServiceStandard.ShowMessage(ex.Message);
            //}
            // Общие ошибки
            catch (Exception ex)
            {
                ApplicationAbortionMethod?.Invoke();
                DialogServiceStandard.ShowMessage(ex.Message);
            }          
        }

        /// <summary>
        /// Обертка для вызова индикатора загрузки и отлова ошибок асинхронной функции
        /// </summary> 
        public async Task ExecuteAndHandleErrorAsync<T1>(Func<T1, Task> functionAsync, T1 arg1, Action ApplicationAbortionMethod = null)
        {
            await ExecuteAndHandleErrorAsync(() => functionAsync(arg1), ApplicationAbortionMethod);
        }
    }
}
