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
        public ExecuteAndCatchErrors()
        {          
        }

        /// <summary>
        ///Отлов ошибок и вызов постметода       
        /// </summary> 
        public IErrorConverting ExecuteAndHandleError(Action method,
                                          Action applicationBeforeMethod = null,
                                          Action applicationCatchMethod = null,
                                          Action applicationFinallyMethod = null)
        {
            IErrorConverting errorConverting = null;

            try
            {
                applicationBeforeMethod?.Invoke();
                method();
            }
            catch (Exception ex)
            {
                applicationCatchMethod?.Invoke();              
                errorConverting = new ErrorConverting(GetTypeException(ex),
                                                      errorConverting?.ErrorDescription, ex.Message, ex.StackTrace);
            }
            finally
            {
                applicationFinallyMethod?.Invoke();
            }

            return errorConverting;
        }

        /// <summary>
        ///Отлов ошибок и вызов постметода асинхронно     
        /// </summary> 
        public async Task<IErrorConverting> ExecuteAndHandleErrorAsync(Func<Task> asyncMethod,
                                                     Action applicationBeforeMethod = null,
                                                     Action applicationCatchMethod = null,
                                                     Action applicationFinallyMethod = null)
        {
            IErrorConverting errorConverting = null;

            try
            {
                applicationBeforeMethod?.Invoke();
                await asyncMethod();
            }
            catch (Exception ex)
            {
                applicationCatchMethod?.Invoke();
                errorConverting = new ErrorConverting(GetTypeException(ex),
                                                     errorConverting?.ErrorDescription, ex.Message, ex.StackTrace);                         
            }
            finally
            {
                applicationFinallyMethod?.Invoke();
            }

            return errorConverting;
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
