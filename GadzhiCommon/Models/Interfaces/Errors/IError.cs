using GadzhiCommon.Enums.FilesConvert;
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
    public interface IErrorCommon: IEnumerable<IErrorCommon>
    {
        /// <summary>
        /// Тип ошибки при конвертации файлов
        /// </summary>
        FileConvertErrorType FileConvertErrorType { get; }

        /// <summary>
        /// Описание ошибки
        /// </summary>
        string ErrorDescription { get; }

        /// <summary>
        /// Исключение
        /// </summary>
        string ExceptionMessage { get; }

        /// <summary>
        /// Стек вызовов
        /// </summary>
        string StackTrace { get; }

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
