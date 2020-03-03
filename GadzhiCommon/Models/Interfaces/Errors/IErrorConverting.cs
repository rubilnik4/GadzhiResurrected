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
    public interface IErrorConverting
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
    }
}
