using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiApplicationCommon.Extensions.Functional.Result
{
    /// <summary>
    /// Преобразование внутреннего типа результирующего ответа
    /// </summary>
    public static class ResultMapExtensions
    { 
        /// <summary>
        /// Выполнить действие при положительном условии, вернуть результирующий ответ
        /// </summary>      
        public static IResultAppValue<TValue> ResultVoidOk<TValue>(this IResultAppValue<TValue> @this, Action<TValue> action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));

            if (@this?.OkStatus == true) action.Invoke(@this.Value);
            return @this;
        }
    }
}
