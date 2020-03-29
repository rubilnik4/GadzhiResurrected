using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Models.Interfaces.Errors;
using System.Collections.Generic;

namespace GadzhiCommon.Infrastructure.Interfaces
{
    /// <summary>
    /// Класс для отображения изменений и логгирования
    /// </summary>
    public interface IMessagingService
    {
        /// <summary>
        /// Отобразить и записать в журнал сообщение
        /// </summary>     
        void ShowAndLogMessage(string message);

        /// <summary>
        /// Отобразить и добавить в журнал ошибку
        /// </summary>       
        void ShowAndLogError(IErrorConverting errorConverting);

        /// <summary>
        /// Отобразить и добавить в журнал ошибки
        /// </summary>       
        void ShowAndLogErrors(IEnumerable<IErrorConverting> errorsConverting);
    }
}
