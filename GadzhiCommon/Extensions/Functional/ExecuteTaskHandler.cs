using System;
using System.Threading.Tasks;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiCommon.Models.Interfaces.Errors;
using static GadzhiCommon.Infrastructure.Implementations.ExecuteAndCatchErrors;

namespace GadzhiCommon.Extensions.Functional
{
    /// <summary>
    /// Отлов ошибок и их суммирование с привязкой к результирующему ответу с вложенной асинхронностью
    /// </summary> 
    public static class ExecuteTaskHandler
    {
        /// <summary>
        /// Отлов ошибок и суммирование ошибок для модуля конвертации   
        /// </summary> 
        public static IResultValue<Task<T>> ExecuteBindResultValueAsync<T>(Func<Task<T>> method)
        {
            if (method == null) throw new ArgumentNullException(nameof(method));

            try
            {
                var result = new ResultValue<Task<T>>(method.Invoke());
                return result;
            }
            catch (Exception ex)
            {
                return new ErrorCommon(GetTypeException(ex), String.Empty, ex).
                       ToResultValue<Task<T>>();
            }
        }
    }
}
