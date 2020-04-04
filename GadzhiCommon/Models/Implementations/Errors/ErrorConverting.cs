using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Models.Interfaces.Errors;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiCommon.Models.Implementations.Errors
{
    /// <summary>
    /// Ошибка конвертации
    /// </summary>
    public class ErrorConverting: IErrorConverting
    {
        public ErrorConverting(FileConvertErrorType fileConvertErrorType, string errorDescription)
            : this(fileConvertErrorType, errorDescription, null, null)
        {
          
        }

        public ErrorConverting(FileConvertErrorType fileConvertErrorType,
                               string errorDescription,
                               string exceptionMessage,
                               string stackTrace)
        {
            FileConvertErrorType = fileConvertErrorType;
            ErrorDescription = errorDescription ?? string.Empty;
            ExceptionMessage = exceptionMessage;
            StackTrace = stackTrace;
        }

        /// <summary>
        /// Тип ошибки при конвертации файлов
        /// </summary>
        public FileConvertErrorType FileConvertErrorType { get; }

        /// <summary>
        /// Описание ошибки
        /// </summary>
        public string ErrorDescription { get; }

        /// <summary>
        /// Исключение
        /// </summary>
        public string ExceptionMessage { get; }

        /// <summary>
        /// Стек вызовов
        /// </summary>
        public string StackTrace { get; }

        /// <summary>
        /// Преобразовать в ответ
        /// </summary>      
        public IResultConverting ToResultConverting() => new ResultConverting(this);

        /// <summary>
        /// Преобразовать в ответ с вложенным типом
        /// </summary>      
        public IResultConvertingValue<TValue> ToResultConvertingValue<TValue>() => new ResultConvertingValue<TValue>(this);

        /// <summary>
        /// Реализация перечисления
        /// </summary>       
        public IEnumerator<IErrorConverting> GetEnumerator()
        {
            yield return this;
        }

        /// <summary>
        /// Реализация перечисления
        /// </summary>  
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();   
        
    }
}
