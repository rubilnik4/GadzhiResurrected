using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Extentions.Functional;
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
        public ExecuteAndCatchErrors() { }

        /// <summary>
        /// Отлов ошибок и вызов постметода       
        /// </summary> 
        public IResultConverting ExecuteAndHandleError(Action method, Action applicationBeforeMethod = null,
                                                       Action applicationCatchMethod = null, Action applicationFinallyMethod = null)
        {
            IResultConverting result = new ResultConverting();

            try
            {
                applicationBeforeMethod?.Invoke();
                method?.Invoke();
            }
            catch (Exception ex)
            {
                applicationCatchMethod?.Invoke();
                result = new ErrorConverting(GetTypeException(ex), String.Empty, ex.Message, ex.StackTrace).ToResultConverting();
            }
            finally
            {
                applicationFinallyMethod?.Invoke();
            }

            return result;
        }

        /// <summary>
        /// Отлов ошибок и суммирование ошибок     
        /// </summary> 
        public IResultConvertingValue<TSource> ExecuteBindErrors<TSource>(Func<IResultConvertingValue<TSource>> method)
        {
            if (method == null) throw new ArgumentNullException(nameof(method));
            IResultConvertingValue<TSource> result = new ResultConvertingValue<TSource>();

            try
            {
                result = method.Invoke();
                
            }
            catch (Exception ex)
            {               
                result = result.ConcatResult
                         new ErrorConverting(GetTypeException(ex), String.Empty, ex.Message, ex.StackTrace).ToResultConverting();
            }         

            return result;
        }

        /// <summary>
        /// Отлов ошибок и вызов постметода асинхронно     
        /// </summary> 
        public async Task<IResultConverting> ExecuteAndHandleErrorAsync(Func<Task> asyncMethod, Action applicationBeforeMethod = null,
                                                                        Action applicationCatchMethod = null, Action applicationFinallyMethod = null)
        {
            IResultConverting result = new ResultConverting();

            try
            {
                applicationBeforeMethod?.Invoke();
                await asyncMethod?.Invoke();
            }
            catch (Exception ex)
            {
                applicationCatchMethod?.Invoke();
                result = new ErrorConverting(GetTypeException(ex), String.Empty, ex.Message, ex.StackTrace).ToResultConverting();
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
        private FileConvertErrorType GetTypeException(Exception ex)
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
