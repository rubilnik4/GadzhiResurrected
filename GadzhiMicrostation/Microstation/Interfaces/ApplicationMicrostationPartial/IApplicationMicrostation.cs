using GadzhiMicrostation.Infrastructure.Interfaces;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Models.Implementations.Coordinates;
using GadzhiMicrostation.Models.Interfaces;
using System;

namespace GadzhiMicrostation.Microstation.Interfaces.ApplicationMicrostationPartial
{
    /// <summary>
    /// Класс для работы с приложением Microstation
    /// </summary>
    public interface IApplicationMicrostation : IApplicationMicrostationDesingFile, IApplicationMicrostationCommands, 
                                                IApplicationMicrostationPrinting, IDisposable
    {  
        /// <summary>
        /// Загрузилась ли оболочка Microstation
        /// </summary>
        bool IsApplicationValid { get; }

        /// <summary>
        /// Текущий файл Microstation
        /// </summary>
        IDesignFileMicrostation ActiveDesignFile { get; }

        /// <summary>
        /// Сервис работы с ошибками
        /// </summary>
        IMessagingMicrostationService MessagingMicrostationService { get; }       

        /// <summary>
        /// Закрыть приложение
        /// </summary>
        void CloseApplication();  
    }
}
