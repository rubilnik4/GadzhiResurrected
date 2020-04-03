using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiConverting.Models.Converters;
using GadzhiConverting.Models.Interfaces;
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
        /// Преобразовать результирующий отвеа модуля конвертации в основной
        /// </summary>      
        public static IResultFileDataSource ToResultConverting(this IResultApplication resultApplication) =>
          ResultApplicationConverter.ToResultConverting(resultApplication);
    }
}
