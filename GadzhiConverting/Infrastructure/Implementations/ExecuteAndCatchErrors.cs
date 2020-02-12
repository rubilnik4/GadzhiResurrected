using GadzhiCommon.Enums.FilesConvert;
using GadzhiConverting.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiConverting.Infrastructure.Implementations
{
    /// <summary>
    /// Класс обертка для отлова ошибок
    /// </summary> 
    public class ExecuteAndCatchErrors : IExecuteAndCatchErrors
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
                                          Action ApplicationBeforeMethod = null,
                                          Action ApplicationFinallyMethod = null)
        {
            try
            {
                ApplicationBeforeMethod?.Invoke();
                method();
            }
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
            catch (Exception ex)
            {
                FileConvertErrorType fileConvertErrorType = FileConvertErrorType.UnknownError;

                if (ex is TimeoutException timeoutException)
                {
                    fileConvertErrorType = FileConvertErrorType.TimeOut;
                }
                else if (ex is CommunicationException communicationException)
                {
                    fileConvertErrorType = FileConvertErrorType.TimeOut;
                }   

                _messageAndLoggingService.ShowError(fileConvertErrorType,
                                                    ex.Message + "\n" + ex.StackTrace);
            }
            finally
            {
                ApplicationFinallyMethod?.Invoke();
            }
        }

        ///// <summary>
        /////Отлов ошибок и вызов постметода       
        ///// </summary> 
        //public void ExecuteAndHandleError<T1>(Action<T1> function, 
        //                                      T1 arg1,
        //                                      Action ApplicationBeforeMethod = null,
        //                                      Action ApplicationFinallyMethod = null)
        //{
        //    ExecuteAndHandleError(() => function(arg1), ApplicationFinallyMethod);
        //}

        /// <summary>
        ///Отлов ошибок и вызов постметода асинхронно     
        /// </summary> 
        public async Task ExecuteAndHandleErrorAsync(Func<Task> asyncMethod,
                                                     Action ApplicationBeforeMethod = null,
                                                     Action ApplicationFinallyMethod = null)
        {
            try
            {
                ApplicationBeforeMethod?.Invoke();
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

        ///// <summary>
        /////Отлов ошибок и вызов постметода асинхронно     
        ///// </summary> 
        //public async Task ExecuteAndHandleErrorAsync<T1>(Func<T1, Task> functionAsync, 
        //                                                 T1 arg1,
        //                                                 Action ApplicationBeforeMethod = null,
        //                                                 Action ApplicationFinallyMethod = null)
        //{
        //    await ExecuteAndHandleErrorAsync(() => functionAsync(arg1), ApplicationFinallyMethod);
        //}
    }
}
