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
            catch (Exception ex)
            {
                catchMethod?.Invoke();
                result = new ErrorCommon(GetTypeException(ex), String.Empty, ex.Message, ex.StackTrace).
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
            catch (Exception ex)
            {
                catchMethod?.Invoke();
                result = new ErrorCommon(GetTypeException(ex), String.Empty, ex.Message, ex.StackTrace).ToResult();
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
                TimeoutException _ => FileConvertErrorType.TimeOut,
                CommunicationException _ => FileConvertErrorType.Communication,
                _ => FileConvertErrorType.UnknownError
            };
    }
}
