using GadzhiCommon.Extentions.Functional;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiCommon.Models.Interfaces.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GadzhiCommon.Infrastructure.Implementations.ExecuteAndCatchErrors;

namespace GadzhiConverting.Infrastructure.Implementations
{
    /// <summary>
    /// Отлов ошибок и суммирование ошибок для модуля конвертации   
    /// </summary> 
    public static class ExecuteBindHandler
    {
        /// <summary>
        /// Отлов ошибок и суммирование ошибок для модуля конвертации   
        /// </summary> 
        public static TResult ExecuteBindFileDataErrors<T, TResult>(Func<TResult> method, IErrorCommon errorMessage = null)
            where TResult: IResultValue<T>
        {
            if (method == null) throw new ArgumentNullException(nameof(method));
            IResultValue<T> result = default(TResult);

            try
            {
                result = method.Invoke();
            }
            catch (Exception ex)
            {
                result = new ErrorCommon(GetTypeException(ex), String.Empty, ex.Message, ex.StackTrace).
                         Map(error => result.ConcatErrors(error).ConcatErrors(errorMessage));                     
            }

            return (TResult)result;
        }       
    }
}
