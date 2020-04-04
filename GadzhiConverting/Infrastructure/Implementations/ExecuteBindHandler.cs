using GadzhiCommon.Extentions.Functional;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiCommon.Models.Interfaces.Errors;
using GadzhiConverting.Extensions;
using GadzhiConverting.Models.Implementations;
using GadzhiConverting.Models.Interfaces;
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
        public static IResultFileDataSource ExecuteBindFileDataErrors(Func<IResultFileDataSource> method, IErrorConverting errorMessage = null)
        {
            if (method == null) throw new ArgumentNullException(nameof(method));
            IResultFileDataSource result = new ResultFileDataSource();

            try
            {
                result = method.Invoke();
            }
            catch (Exception ex)
            {
                result = new ErrorConverting(GetTypeException(ex), String.Empty, ex.Message, ex.StackTrace).
                         Map(error => result.ConcatErrors(error).ConcatErrors(errorMessage)).
                         ToResultFileDataSource();
            }

            return result;
        }
    }
}
