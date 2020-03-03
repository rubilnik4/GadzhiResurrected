using GadzhiMicrostation.Infrastructure.Interfaces;
using GadzhiMicrostation.Models.Enums;
using GadzhiMicrostation.Models.Implementations;
using System;


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
        private readonly IMessagingMicrostationService _errorMessagingMicrostation;

        public ExecuteAndCatchErrorsMicrostation(IMessagingMicrostationService errorMessagingMicrostation)
        {
            _errorMessagingMicrostation = errorMessagingMicrostation;
        }

        /// <summary>
        ///Отлов ошибок и вызов постметода       
        /// </summary> 
        public void ExecuteAndHandleError(Action method,
                                          Action applicationBeforeMethod = null,
                                          Func<ErrorMicrostation> applicationCatchMethod = null,
                                          Action applicationFinallyMethod = null)
        {
            try
            {
                applicationBeforeMethod?.Invoke();
                method();
            }
            catch (Exception ex)
            {
                var errorMicrostation = applicationCatchMethod?.Invoke();

                ErrorMicrostationType fileConvertErrorType = GetTypeException(ex, errorMicrostation.ErrorMicrostationType);
                _errorMessagingMicrostation.ShowAndLogError(new ErrorMicrostation(fileConvertErrorType, errorMicrostation?.ErrorDescription,
                                                                                  ex.Message, ex.StackTrace));
            }
            finally
            {
                applicationFinallyMethod?.Invoke();
            }
        }

        /// <summary>
        /// Получить тип ошибки
        /// </summary>       
        private ErrorMicrostationType GetTypeException(Exception ex, ErrorMicrostationType? fileConvertErrorTypeNull = null)
        {
            ErrorMicrostationType fileConvertErrorType = ErrorMicrostationType.UnknownError;
            if (fileConvertErrorTypeNull != null && fileConvertErrorTypeNull != ErrorMicrostationType.UnknownError)
            {
                fileConvertErrorType = fileConvertErrorTypeNull.Value;
            }

            if (fileConvertErrorType != ErrorMicrostationType.UnknownError)
            {
                if (ex is NullReferenceException)
                {
                    fileConvertErrorType = ErrorMicrostationType.NullReference;
                }
                else if (ex is ArgumentNullException)
                {
                    fileConvertErrorType = ErrorMicrostationType.ArgumentNullReference;
                }
            }

            return fileConvertErrorType;
        }
    }
}
