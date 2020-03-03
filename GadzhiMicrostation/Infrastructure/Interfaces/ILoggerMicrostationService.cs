using GadzhiMicrostation.Models.Implementations;

namespace GadzhiMicrostation.Infrastructure.Interfaces
{
    /// <summary>
    /// Запись в журнал системных сообщений
    /// </summary>
    public interface ILoggerMicrostationService
    {
        /// <summary>
        /// Записать ошибку
        /// </summary>        
        void LogError(ErrorMicrostation errorMicrostation);

        /// <summary>
        /// Записать сообщение
        /// </summary>
        void LogMessage(string message);
    }
}
