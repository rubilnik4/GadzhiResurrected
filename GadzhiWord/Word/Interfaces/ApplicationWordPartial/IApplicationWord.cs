using GadzhiCommon.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiWord.Word.Interfaces.ApplicationWordPartial
{
    /// <summary>
    /// Класс для работы с приложением Word
    /// </summary>
    public interface IApplicationWord: IApplicationWordDocument
    {
        /// <summary>
        /// Класс для отображения изменений и логгирования
        /// </summary>
        IMessagingService MessagingService { get; }

        /// <summary>
        /// Загрузилась ли оболочка Microstation
        /// </summary>
        bool IsApplicationValid { get; }
    }
}
