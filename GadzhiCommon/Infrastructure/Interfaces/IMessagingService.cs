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
        void ShowMessage(string message);

        /// <summary>
        /// Отобразить и добавить в журнал ошибку
        /// </summary>       
        void ShowError(IErrorCommon errorConverting);

        /// <summary>
        /// Отобразить и добавить в журнал ошибки
        /// </summary>       
        void ShowErrors(IEnumerable<IErrorCommon> errorsConverting);

        /// <summary>
        /// Отобразить и записать в лог ошибку
        /// </summary>
        void ShowAndLogError(IErrorCommon errorConverting);

        /// <summary>
        /// Отобразить и записать в лог ошибки
        /// </summary>
        void ShowAndLogErrors(IEnumerable<IErrorCommon> errorsConverting);
    }
}
