using GadzhiMicrostation.Models.Enums;

namespace GadzhiMicrostation.Models.Implementations
{
    /// <summary>
    /// Ошибка Microstation
    /// </summary>
    public class ErrorMicrostation
    {
        public ErrorMicrostation(ErrorMicrostationType errorMicrostationType, string errorDescription)
           : this(errorMicrostationType, errorDescription, null, null)
        {

        }

        public ErrorMicrostation(ErrorMicrostationType errorMicrostationType,
                                 string errorDescription,
                                 string exceptionMessage,
                                 string stackTrace)
        {
            ErrorMicrostationType = errorMicrostationType;
            ErrorDescription = errorDescription ?? string.Empty;
            ExceptionMessage = exceptionMessage;
            StackTrace = stackTrace;
        }

        /// <summary>
        /// Тип ошибки при конвертации Microstation
        /// </summary>
        public ErrorMicrostationType ErrorMicrostationType { get; }

        /// <summary>
        /// Описание ошибки
        /// </summary>
        public string ErrorDescription { get; }

        /// <summary>
        /// Исключение
        /// </summary>
        public string ExceptionMessage { get; }

        /// <summary>
        /// Стэк вызовов
        /// </summary>
        public string StackTrace { get; }
    }
}
