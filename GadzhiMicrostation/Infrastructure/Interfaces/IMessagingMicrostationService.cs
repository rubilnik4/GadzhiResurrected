using GadzhiMicrostation.Models.Implementations;
using System.Collections.Generic;

namespace GadzhiMicrostation.Infrastructure.Interfaces
{
    /// <summary>
    /// Класс для отображения изменений и логгирования Microstation
    /// </summary>
    public interface IMessagingMicrostationService
    {    
        /// <summary>
        /// Отобразить и добавить в журнал ошибку
        /// </summary>      
        void ShowAndLogError(ErrorMicrostation errorMicrostation);

        /// <summary>
        /// Отобразить и записать в журнал сообщение
        /// </summary>
        void ShowAndLogMessage(string message);       
    }
}
