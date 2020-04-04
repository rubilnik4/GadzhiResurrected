using ConvertingModels.Models.Interfaces.FilesConvert;
using GadzhiApplicationCommon.Models.Interfaces.Errors;
using GadzhiCommon.Extentions.Functional;
using GadzhiCommon.Models.Interfaces.Errors;
using GadzhiConverting.Models.Converters;
using GadzhiConverting.Models.Implementations;
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
        /// Преобразовать результирующий ответ модуля конвертации в основной
        /// </summary>      
        public static IResultFileDataSource ToResultFileDataSource(this IResultApplication resultApplication) =>
          ResultApplicationConverter.ToResultFileDataSource(resultApplication);

        /// <summary>
        /// Преобразовать ответ с параметров в ответ модуля
        /// </summary>      
        public static IResultFileDataSource ToResultFileDataSource(this IResultConvertingValue<IEnumerable<IFileDataSourceServer>> resultConverting) =>
          resultConverting?.
          Map(result => new ResultFileDataSource(result.Value, result.Errors))
          ?? throw new ArgumentNullException(nameof(resultConverting));
    }
}
