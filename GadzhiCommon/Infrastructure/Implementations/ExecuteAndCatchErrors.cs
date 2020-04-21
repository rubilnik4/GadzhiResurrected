using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Extentions.Functional;
using GadzhiCommon.Functional;
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
    public static class ExecuteAndCatchErrors
    {
        /// <summary>
        /// Отлов ошибок и вызов постметода       
        /// </summary> 
        public static IResultError ExecuteAndHandleError(Action method, Action applicationBeforeMethod = null,
                                                               Action applicationCatchMethod = null, Action applicationFinallyMethod = null,
                                                                IErrorCommon errorMessage = null) =>
            ExecuteAndHandleError(() => { method.Invoke(); return Unit.Value; },
                                  applicationBeforeMethod, applicationCatchMethod,
                                  applicationFinallyMethod, errorMessage).
            ToResult();

        /// <summary>
        /// Отлов ошибок и вызов постметода       
        /// </summary> 
        public static IResultValue<T> ExecuteAndHandleError<T>(Func<T> method, Action applicationBeforeMethod = null,
                                                               Action applicationCatchMethod = null, Action applicationFinallyMethod = null,
                                                                IErrorCommon errorMessage = null)
        {
            IResultValue<T> result = default;

            try
            {
                applicationBeforeMethod?.Invoke();
                result = new ResultValue<T>(method.Invoke());
            }
            catch (Exception ex)
            {
                applicationCatchMethod?.Invoke();
                result = new ErrorCommon(GetTypeException(ex), String.Empty, ex.Message, ex.StackTrace).
                         ToResultValue<T>().
                         ConcatErrors(errorMessage);
            }
            finally
            {
                applicationFinallyMethod?.Invoke();
            }

            return result;
        }

        /// <summary>
        /// Отлов ошибок и вызов постметода асинхронно     
        /// </summary> 
        public static async Task<IResultError> ExecuteAndHandleErrorAsync(Func<Task> asyncMethod, Action applicationBeforeMethod = null,
                                                                               Action applicationCatchMethod = null,
                                                                               Action applicationFinallyMethod = null)
        {
            IResultError result = new ResultError();

            try
            {
                applicationBeforeMethod?.Invoke();
                await asyncMethod?.Invoke();
            }
            catch (Exception ex)
            {
                applicationCatchMethod?.Invoke();
                result = new ErrorCommon(GetTypeException(ex), String.Empty, ex.Message, ex.StackTrace).ToResult();
            }
            finally
            {
                applicationFinallyMethod?.Invoke();
            }

            return result;
        }

        /// <summary>
        /// Получить тип ошибки
        /// </summary>       
        public static FileConvertErrorType GetTypeException(Exception ex)
        {
            FileConvertErrorType fileConvertErrorType = FileConvertErrorType.UnknownError;

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
