﻿using System;
using System.Collections.Generic;
using System.Linq;
using GadzhiCommon.Converters;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiCommon.Models.Interfaces.Errors;

namespace GadzhiCommon.Extensions.Functional
{
    /// <summary>
    /// Методы расширения для ошибок
    /// </summary>
    public static class ErrorExtension
    {
        /// <summary>
        /// Преобразовать коллекцию ошибок в результирующий ответ с параметром
        /// </summary>       
        public static IResultError ToResult(this IEnumerable<IErrorCommon> @this) =>
            new ResultError(@this);

        /// <summary>
        /// Преобразовать коллекцию ошибок в результирующий ответ с параметром
        /// </summary>       
        public static IResultValue<TValue> ToResultValue<TValue>(this IEnumerable<IErrorCommon> @this) =>
            new ResultValue<TValue>(@this);
    }
}