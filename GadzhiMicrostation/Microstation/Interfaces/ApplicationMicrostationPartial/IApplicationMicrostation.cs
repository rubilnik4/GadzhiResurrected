using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Models.Coordinates;
using System;

namespace GadzhiMicrostation.Microstation.Interfaces.ApplicationMicrostationPartial
{
    /// <summary>
    /// Класс для работы с приложением Microstation
    /// </summary>
    public interface IApplicationMicrostation : IApplicationMicrostationDesingFile, IApplicationMicrostationCommands
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
        /// Закрыть приложение
        /// </summary>
        void CloseApplication();  
    }
}
