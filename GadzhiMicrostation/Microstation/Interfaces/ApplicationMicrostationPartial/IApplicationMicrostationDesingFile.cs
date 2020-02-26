using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Microstation.Interfaces.ApplicationMicrostationPartial
{
    /// <summary>
    /// Подкласс приложения Microstation для работы с файлом
    /// </summary>
    public interface IApplicationMicrostationDesingFile
    {  
        /// <summary>
        /// Открыть файл
        /// </summary>       
        IDesignFileMicrostation OpenDesignFile(string filePath);

        /// <summary>
        /// Сохранить файл
        /// </summary>       
        void SaveDesignFile(string filePath);

        /// <summary>
        /// Сохранить файл PDF
        /// </summary>
        void CreatePdfFile(string filePath);

        /// <summary>
        /// Создать файл типа DWG
        /// </summary>
        void CreateDwgFile(string filePath);

        /// <summary>
        /// Закрыть файл
        /// </summary>
        void CloseDesignFile();
    }
}
