using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiCommon.Models.Interfaces.Errors;
using System;
using System.ServiceModel;
using System.Threading.Tasks;

namespace GadzhiCommon.Infrastructure.Implementations
{
    /// <summary>
    /// Класс обертка для отлова ошибок
    /// </summary> 
    public class ExecuteAndCatchErrors : IExecuteAndCatchErrors
    {
        /// <summary>
        /// Стандартные диалоговые окна
        /// </summary> 
        private readonly IMessagingService _messagingService;

        public ExecuteAndCatchErrors(IMessagingService messagingService)
        {
            _messagingService = messagingService;
        }

        /// <summary>
        ///Отлов ошибок и вызов постметода       
        /// </summary> 
        public void ExecuteAndHandleError(Action method,
                                          Action applicationBeforeMethod = null,
                                          Func<IErrorConverting> applicationCatchMethod = null,
                                          Action applicationFinallyMethod = null)
        {
            try
            {
                applicationBeforeMethod?.Invoke();
                method();
            }
            catch (Exception ex)
            {
                var errorConverting = applicationCatchMethod?.Invoke();

                FileConvertErrorType fileConvertErrorType = GetTypeException(ex, errorConverting.FileConvertErrorType);
                _messagingService.ShowAndLogError(new ErrorConverting(fileConvertErrorType, errorConverting?.ErrorDescription,
                                                                      ex.Message, ex.StackTrace));
            }
            finally
            {
                applicationFinallyMethod?.Invoke();
            }
        }

        /// <summary>
        ///Отлов ошибок и вызов постметода асинхронно     
        /// </summary> 
        public async Task ExecuteAndHandleErrorAsync(Func<Task> asyncMethod,
                                                     Action applicationBeforeMethod = null,
                                                     Func<Task<IErrorConverting>> applicationCatchMethod = null,
                                                     Action applicationFinallyMethod = null)
        {
            try
            {
                applicationBeforeMethod?.Invoke();
                await asyncMethod();
            }
            catch (Exception ex)
            {
                var errorConverting = await applicationCatchMethod?.Invoke();

                FileConvertErrorType fileConvertErrorType = GetTypeException(ex, errorConverting.FileConvertErrorType);
                _messagingService.ShowAndLogError(new ErrorConverting(fileConvertErrorType, errorConverting?.ErrorDescription,
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
        private FileConvertErrorType GetTypeException(Exception ex, FileConvertErrorType? fileConvertErrorTypeNull = null)
        {
            FileConvertErrorType fileConvertErrorType = FileConvertErrorType.UnknownError;
            if (fileConvertErrorTypeNull != null && fileConvertErrorTypeNull != FileConvertErrorType.UnknownError)
            {
                fileConvertErrorType = fileConvertErrorTypeNull.Value;
            }

            if (fileConvertErrorType != FileConvertErrorType.UnknownError)
            {
                if (ex is NullReferenceException)
                {
                    fileConvertErrorType = FileConvertErrorType.NullReference;
                }
                else if (ex is ArgumentNullException)
                {
                    fileConvertErrorType = FileConvertErrorType.ArgumentNullReference;
                }
                else if (ex is FormatException)
                {
                    fileConvertErrorType = FileConvertErrorType.FormatException;
                }
                else if (ex is TimeoutException)
                {
                    fileConvertErrorType = FileConvertErrorType.TimeOut;
                }
                else if (ex is CommunicationException)
                {
                    fileConvertErrorType = FileConvertErrorType.Communication;
                }
            }
            return fileConvertErrorType;
        }
    }
}
