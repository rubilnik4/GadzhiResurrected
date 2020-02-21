using GadzhiCommon.Enums.FilesConvert;

namespace GadzhiCommon.Infrastructure.Interfaces
{
    /// <summary>
    /// Класс для отображения изменений и логгирования
    /// </summary>
    public interface IMessageAndLoggingService
    {
        /// <summary>
        /// Отобразить ошибку
        /// </summary>        
        void ShowError(FileConvertErrorType fileConvertErrorType,
                       string fileConvertErrorDescription);

        /// <summary>
        /// </summary>        
        /// </summary>        
        void ShowMessage(string message);
    }
}
