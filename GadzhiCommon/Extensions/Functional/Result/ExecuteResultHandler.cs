using System;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiCommon.Models.Interfaces.Errors;
using static GadzhiCommon.Infrastructure.Implementations.ExecuteAndCatchErrors;

namespace GadzhiCommon.Extensions.Functional.Result
{
    /// <summary>
    /// Отлов ошибок и их суммирование с привязкой к результирующему ответу  
    /// </summary> 
    public static class ExecuteResultHandler
    {
        /// <summary>
        /// Отлов ошибок и суммирование ошибок для модуля конвертации   
        /// </summary> 
        public static IResultValue<T> ExecuteBindResultValue<T>(Func<IResultValue<T>> method, IErrorCommon errorMessage = null)
        {
            if (method == null) throw new ArgumentNullException(nameof(method));
            IResultValue<T> result;
            try
            {
                result = method.Invoke();
            }
            catch (Exception ex)
            {
                result = new ErrorCommon(GetTypeException(ex), String.Empty, ex).
                         ToResultValue<T>().
                         ConcatErrors(errorMessage);
            }

            return result;
        }       
    }
}
