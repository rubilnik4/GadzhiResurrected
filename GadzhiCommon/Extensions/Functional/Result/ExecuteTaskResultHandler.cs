using System;
using System.Threading.Tasks;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiCommon.Models.Interfaces.Errors;
using static GadzhiCommon.Infrastructure.Implementations.ExecuteAndCatchErrors;

namespace GadzhiCommon.Extensions.Functional.Result
{
    /// <summary>
    /// Отлов ошибок и их суммирование с привязкой к результирующему ответу с вложенной асинхронностью
    /// </summary> 
    public static class ExecuteTaskResultHandler
    {
        /// <summary>
        /// Отлов ошибок и суммирование ошибок для модуля конвертации   
        /// </summary> 
        public static async Task<IResultValue<T>> ExecuteBindResultValueAsync<T>(Func<Task<T>> method)
        {
            try
            {
                var result =await method.Invoke();
                return new ResultValue<T>(result);
            }
            catch (Exception ex)
            {
                return new ErrorCommon(GetTypeException(ex), String.Empty, ex).
                       ToResultValue<T>();
            }
        }
    }
}
