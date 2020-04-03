using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiCommon.Extentions.Functional;
using GadzhiConverting.Extensions;
using GadzhiConverting.Models.Implementations;
using GadzhiConverting.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiConverting.Models.Converters
{
    /// <summary>
    /// Преобразование результирующего ответа модуля конвертации в основной
    /// </summary>
    public static class ResultApplicationConverter
    {
        /// <summary>
        /// Преобразовать результирующий отвеа модуля конвертации в основной
        /// </summary>      
        public static IResultFileDataSource ToResultConverting(IResultApplication resultApplication) =>
            resultApplication?.
            Map(result => new ResultFileDataSource(result.ErrorsApplication.ToErrorsConverting()))
            ?? throw new ArgumentNullException(nameof(resultApplication));


    }
}
