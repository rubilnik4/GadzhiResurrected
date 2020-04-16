using GadzhiCommon.Models.Implementations.Errors;
using GadzhiCommon.Models.Interfaces.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GadzhiCommon.Infrastructure.Implementations.ExecuteAndCatchErrors;

namespace GadzhiCommon.Extentions.Functional
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
                return new ErrorCommon(GetTypeException(ex), String.Empty, ex.Message, ex.StackTrace).
                       ToResultValue<Task<T>>();
            }
        }
    }
}
