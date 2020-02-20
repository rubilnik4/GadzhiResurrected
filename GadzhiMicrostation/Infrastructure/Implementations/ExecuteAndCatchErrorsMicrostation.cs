using GadzhiMicrostation.Infrastructure.Interface;
using GadzhiMicrostation.Infrastructure.Interfaces;
using GadzhiMicrostation.Models.Enum;
using GadzhiMicrostation.Models.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;


namespace GadzhiMicrostation.Infrastructure.Implementations
{
    /// <summary>
    /// Класс обертка для отлова ошибок
    /// </summary> 
    public class ExecuteAndCatchErrorsMicrostation : IExecuteAndCatchErrorsMicrostation
    {
        /// <summary>
        /// Сервис работы с ошибками
        /// </summary>
        private readonly IErrorMessagingMicrostation _errorMessagingMicrostation;

        public ExecuteAndCatchErrorsMicrostation(IErrorMessagingMicrostation errorMessagingMicrostation)
        {
            _errorMessagingMicrostation = errorMessagingMicrostation;
        }

        /// <summary>
        ///Отлов ошибок и вызов постметода       
        /// </summary> 
        public void ExecuteAndHandleError(Action method,
                                          Action applicationBeforeMethod = null,
                                          Action applicationCatchMethod = null,
                                          ErrorMicrostation errorMicrostation = null,
                                          Action applicationFinallyMethod = null)
        {
            try
            {
                applicationBeforeMethod?.Invoke();
                method();
            }
            catch (Exception)
            {
                applicationCatchMethod?.Invoke();

                if (errorMicrostation != null)
                {
                    _errorMessagingMicrostation.AddError(errorMicrostation);
                }
            }
            finally
            {
                applicationFinallyMethod?.Invoke();
            }
        }
    }
}
