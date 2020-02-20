using GadzhiMicrostation.Infrastructure.Interfaces;
using GadzhiMicrostation.Microstation.Interfaces.Elements;
using GadzhiMicrostation.Models.Coordinates;
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

        /// <summary>
        /// Создать ячейку на освнове шаблона в библиотеке
        /// </summary>       
        ICellElementMicrostation CreateCellElementFromLibrary(string cellName, PointMicrostation origin);

        /// <summary>
        /// Создать ячейку на основе шаблона в библиотеке
        /// </summary>       
        ICellElementMicrostation CreateSignatureFromLibrary(string cellName, PointMicrostation origin);
        
    }
}
