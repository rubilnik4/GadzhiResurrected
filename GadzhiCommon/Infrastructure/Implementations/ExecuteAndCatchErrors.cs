using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiCommon.Models.Interfaces.Errors;
using System;
using System.ComponentModel;
using System.ServiceModel;
using System.Threading.Tasks;
using GadzhiCommon.Models.Implementations.Functional;

namespace GadzhiCommon.Infrastructure.Implementations
{
    /// <summary>
    /// Класс обертка для отлова ошибок
    /// </summary> 
    public static class ExecuteAndCatchErrors
    {
        /// <summary>
        /// Отлов ошибок и вызов метода       
        /// </summary> 
        public static IResultError ExecuteAndHandleError(Action method, Action beforeMethod = null, Action catchMethod = null,
                                                         Action finallyMethod = null, IErrorCommon errorMessage = null) =>
            ExecuteAndHandleError(() => { method.Invoke(); return Unit.Value; },
                                  beforeMethod, catchMethod,
                                  finallyMethod, errorMessage).
            ToResult();

        /// <summary>
        /// Отлов ошибок и вызов метода       
        /// </summary> 
        public static IResultValue<T> ExecuteAndHandleError<T>(Func<T> method, Action beforeMethod = null, Action catchMethod = null,
                                                               Action finallyMethod = null, IErrorCommon errorMessage = null)
        {
            if (method == null) throw new ArgumentNullException(nameof(method));

            IResultValue<T> result;

            try
            {
                beforeMethod?.Invoke();
                result = new ResultValue<T>(method.Invoke());
            }
            catch (Exception exception)
            {
                catchMethod?.Invoke();
                result = new ErrorCommon(GetTypeException(exception), String.Empty, exception).
                         ToResultValue<T>().
                         ConcatErrors(errorMessage);
            }
            finally
            {
                finallyMethod?.Invoke();
            }

            return result;
        }

        /// <summary>
        /// Отлов ошибок и вызов метода асинхронно     
        /// </summary> 
        public static async Task<IResultError> ExecuteAndHandleErrorAsync(Func<Task> asyncMethod, Action beforeMethod = null,
                                                                          Action catchMethod = null, Action finallyMethod = null)
        {
            if (asyncMethod == null) throw new ArgumentNullException(nameof(asyncMethod));

            IResultError result = new ResultError();

            try
            {
                beforeMethod?.Invoke();
                await asyncMethod.Invoke();
            }
            catch (Exception exception)
            {
                catchMethod?.Invoke();
                result = new ErrorCommon(GetTypeException(exception), String.Empty, exception).ToResult();
            }
            finally
            {
                finallyMethod?.Invoke();
            }

            return result;
        }

        /// <summary>
        /// Получить тип ошибки
        /// </summary>       
        public static FileConvertErrorType GetTypeException(Exception exception) =>
            exception switch
            {
                NullReferenceException _ => FileConvertErrorType.NullReference,
                ArgumentNullException _ => FileConvertErrorType.ArgumentNullReference,
                FormatException _ => FileConvertErrorType.FormatException,
                InvalidEnumArgumentException _ => FileConvertErrorType.InvalidEnumArgumentException,
                TimeoutException _ => FileConvertErrorType.TimeOut,
                CommunicationException _ => FileConvertErrorType.Communication,
                _ => FileConvertErrorType.UnknownError
            };
    }
}
