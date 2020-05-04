using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiCommon.Models.Interfaces.Errors;
using GadzhiConverting.Models.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiConverting.Extensions
{
    /// <summary>
    /// Преобразование результирующего ответа модуля конвертации в основной
    /// </summary>
    public static class ResultExtensions
    {
        /// <summary>
        /// Преобразовать результирующий ответ модуля конвертации в основной
        /// </summary>      
        public static IResultError ToResultFromApplication(this IResultApplication resultApplication) =>
          ResultApplicationConverter.ToResult(resultApplication);

        /// <summary>
        /// Преобразовать результирующий ответ модуля конвертации со значением в основной
        /// </summary>      
        public static IResultValue<TValue> ToResultValueFromApplication<TValue>(this IResultAppValue<TValue> resultApplication) =>
          ResultApplicationConverter.ToResultValue(resultApplication);

        /// <summary>
        /// Преобразовать результирующий ответ модуля конвертации с коллекцией в основной
        /// </summary>      
        public static IResultCollection<TValue> ToResultCollectionFromApplication<TValue>(this IResultAppCollection<TValue> resultApplication) =>
          ResultApplicationConverter.ToResultCollection(resultApplication);
    }
}
