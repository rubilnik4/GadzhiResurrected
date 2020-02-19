using GadzhiMicrostation.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Microstation.Interfaces
{
    /// <summary>
    /// Класс для работы с приложением Microstation
    /// </summary>
    public interface IApplicationMicrostation
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
        /// Открыть файл
        /// </summary>       
        IDesignFileMicrostation OpenDesignFile(string filePath);

        /// <summary>
        /// Закрыть приложение
        /// </summary>
        void CloseApplication();      

        /// <summary>
        /// Сохранить файл
        /// </summary>       
        void SaveDesignFile(string filePath);


        /// <summary>
        /// Закрыть файл
        /// </summary>
        void CloseDesignFile();
    }
}
