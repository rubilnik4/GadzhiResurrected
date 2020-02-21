using GadzhiMicrostation.Models.Implementations;

namespace GadzhiMicrostation.Infrastructure.Interfaces
{
    /// <summary>
    /// Отображение системных сообщений
    /// </summary>
    public interface ILoggerMicrostation
    {
        /// <summary>
        /// Отобразить ошибку
        /// </summary>        
        void ShowError(ErrorMicrostation errorMicrostation);

        /// <summary>
        /// Отобразить сообщение
        /// </summary>
        void ShowMessage(string message);
    }
}
