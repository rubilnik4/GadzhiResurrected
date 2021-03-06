﻿using GadzhiCommon.Enums.FilesConvert;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiCommon.Models.Interfaces.Errors
{
    /// <summary>
    /// Ошибка конвертации
    /// </summary>
    public interface IErrorCommon: IError, IEnumerable<IErrorCommon>
    {
        /// <summary>
        /// Исключение
        /// </summary>
        Exception Exception { get; }

        /// <summary>
        /// Преобразовать в ответ
        /// </summary>      
        IResultError ToResult();

        /// <summary>
        /// Преобразовать в ответ с вложенным типом
        /// </summary>      
        IResultValue<TValue> ToResultValue<TValue>();
    }
}
